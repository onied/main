using System.Text;
using System.Text.Json;
using ClickHouse.Client.ADO;
using Courses.Data.Abstractions;
using Courses.Services.Abstractions;
using RabbitMQ.Client;

namespace Courses.Services.BackgroundServices;

public class StatsSenderService(IServiceProvider serviceProvider, IModel model) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = serviceProvider.CreateScope();
            var services = scope.ServiceProvider;
            var statsRepository = services.GetRequiredService<IStatsRepository>();
            var courseRepository = services.GetRequiredService<ICourseRepository>();
            var courses = await courseRepository.GetCoursesAsync();
            foreach (var course in courses.TakeWhile(_ => !stoppingToken.IsCancellationRequested))
            {
                var message = JsonSerializer.Serialize(
                    new { courseId = course.Id, likes = await statsRepository.GetCourseLikesAsync(course.Id) }
                );
                var body = Encoding.UTF8.GetBytes(message);
                model.BasicPublish("onied-stats-exchange", string.Empty, body: body);
            }

            await Task.Delay(1000, stoppingToken);
        }
    }
}

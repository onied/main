using Courses.Models;
using MassTransit;
using MassTransit.Data.Messages;
using Task = System.Threading.Tasks.Task;

namespace Courses.Services.Producers.CourseCompletedProducer;

public class CourseCompletedProducer(
    ILogger<CourseCompletedProducer> logger,
    IPublishEndpoint publishEndpoint) : ICourseCompletedProducer
{
    public async Task PublishAsync(Course course, User user)
    {
        await publishEndpoint.Publish(new CourseCompleted(user.Id, course.Id));
        logger.LogInformation("Sent course completed(userId={userId}, courseId={CourseId}",
            user.Id,
            course.Id);
    }
}

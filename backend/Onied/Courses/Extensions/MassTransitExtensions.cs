using Courses.Services.Consumers;
using Courses.Services.Producers.CourseCreatedProducer;
using Courses.Services.Producers.CourseUpdatedProducer;
using MassTransit;

namespace Courses.Extensions;

public static class MassTransitExtensions
{
    public static IServiceCollection AddMassTransitConfigured(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddMassTransit(x =>
        {
            var configuration =
                serviceCollection.BuildServiceProvider()
                    .GetService<IConfiguration>()!;

            x.AddConsumer<UserCreatedConsumer>();
            x.AddConsumer<ProfileUpdatedConsumer>();
            x.AddConsumer<ProfilePhotoUpdatedConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(configuration["RabbitMQ:Host"], configuration["RabbitMQ:VHost"], h =>
                {
                    h.Username(configuration["RabbitMQ:Username"]);
                    h.Password(configuration["RabbitMQ:Password"]);
                });

                cfg.ConfigureEndpoints(context);
            });
        });
        serviceCollection.AddScoped<ICourseCreatedProducer, CourseCreatedProducer>();
        serviceCollection.AddScoped<ICourseUpdatedProducer, CourseUpdatedProducer>();

        return serviceCollection;
    }
}

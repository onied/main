using Courses.Services.Consumers;
using Courses.Services.Producers.CourseCompletedProducer;
using Courses.Services.Producers.CourseCreatedProducer;
using Courses.Services.Producers.CourseUpdatedProducer;
using Courses.Services.Producers.NotificationSentProducer;
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

            x.AddConsumer<UserCreatedConsumer>()
                .Endpoint(e => e.Name = "user-created-courses");
            x.AddConsumer<ProfileUpdatedConsumer>()
                .Endpoint(e => e.Name = "profile-updated-courses");
            x.AddConsumer<ProfilePhotoUpdatedConsumer>()
                .Endpoint(e => e.Name = "profile-photo-updated-courses");
            x.AddConsumer<PurchaseCreatedConsumer>()
                .Endpoint(e => e.Name = "purchase-created-courses");
            x.AddConsumer<SubscriptionChangedConsumer>()
                .Endpoint(e => e.Name = "subscription-changed-consumer");

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(configuration["RabbitMQ:Host"], configuration["RabbitMQ:VHost"], h =>
                {
                    h.Username(configuration["RabbitMQ:Username"]!);
                    h.Password(configuration["RabbitMQ:Password"]!);
                });

                cfg.ConfigureEndpoints(context);
            });
        });
        serviceCollection.AddScoped<ICourseCreatedProducer, CourseCreatedProducer>();
        serviceCollection.AddScoped<ICourseUpdatedProducer, CourseUpdatedProducer>();
        serviceCollection.AddScoped<ICourseCompletedProducer, CourseCompletedProducer>();
        serviceCollection.AddScoped<INotificationSentProducer, NotificationSentProducer>();
        return serviceCollection;
    }
}

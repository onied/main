using MassTransit;
using Purchases.Abstractions;
using Purchases.Consumers;
using Purchases.Producers.PurchaseCreatedProducer;

namespace Purchases.Extensions;

public static class MassTransitExtensions
{
    public static IServiceCollection AddMassTransitConfigured(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IPurchaseCreatedProducer, PurchaseCreatedProducer>();

        serviceCollection.AddMassTransit(x =>
        {
            var configuration =
                serviceCollection.BuildServiceProvider()
                    .GetService<IConfiguration>()!;

            x.AddConsumer<CourseCreatedConsumer>()
                .Endpoint(e => e.Name = "course-created-purchases");
            x.AddConsumer<CourseUpdatedConsumer>()
                .Endpoint(e => e.Name = "course-updated-purchases");
            x.AddConsumer<UserCreatedConsumer>()
                .Endpoint(e => e.Name = "user-created-purchases");

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

        return serviceCollection;
    }
}

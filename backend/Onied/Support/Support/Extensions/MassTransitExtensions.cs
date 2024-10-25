using MassTransit;
using Support.Abstractions;
using Support.Services;

namespace Support.Extensions;

public static class MassTransitExtensions
{
    public static IServiceCollection AddMassTransitConfigured(this IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddMassTransit(x =>
            {
                var configuration =
                    serviceCollection.BuildServiceProvider()
                        .GetService<IConfiguration>()!;

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(configuration["RabbitMQ:Host"], configuration["RabbitMQ:VHost"], h =>
                    {
                        h.Username(configuration["RabbitMQ:Username"]);
                        h.Password(configuration["RabbitMQ:Password"]);
                    });

                    cfg.ConfigureEndpoints(context);
                });
            })
            .AddMassTransit<IMassTransitInMemoryBus>(x =>
            {
                // x.AddConsumer<NotificationSentConsumer>()
                //     .Endpoint(e => e.Name = "notification-sent-notifications");

                x.UsingInMemory();
            });
        serviceCollection.AddScoped<IClientNotificationProducer, ClientNotificationProducer>();
        return serviceCollection;
    }
}

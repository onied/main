using MassTransit;
using Support.Abstractions;
using Support.Services.Consumers;

namespace Support.Extensions;

public static class MassTransitExtensions
{
    public static IServiceCollection AddMassTransitConfigured(this IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddMassTransit(x =>
            {
                x.AddPublishMessageScheduler();

                x.AddHangfireConsumers();

                x.AddConsumer<AbandonChatConsumer>();
                x.AddConsumer<CloseChatConsumer>();
                x.AddConsumer<MarkMessageAsReadConsumer>();
                x.AddConsumer<NewMessageClientNotificationConsumer>();
                x.AddConsumer<SendMessageConsumer>();
                x.AddConsumer<SendMessageToChatConsumer>();

                x.UsingInMemory((context, cfg) =>
                {
                    cfg.UsePublishMessageScheduler();

                    cfg.ConfigureEndpoints(context);
                });
            })
            .AddMassTransit<INotificationBus>(x =>
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
            });
        return serviceCollection;
    }
}

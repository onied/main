using Hangfire;
using Hangfire.MemoryStorage;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Support.Events.Abstractions;
using Support.Events.Consumers;
using Support.Events.Services;

namespace Support.Events.Extensions;

public static class ConfigurationExtensions
{
    public static IServiceCollection AddEventsServices(this IServiceCollection serviceCollection)
    {
        return serviceCollection.AddScoped<IMessageGenerator, MessageGenerator>();
    }

    public static IServiceCollection AddMassTransitWithHangfireConfigured(this IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddHangfire(h =>
            {
                h.UseRecommendedSerializerSettings();
                h.UseMemoryStorage();
            })
            .AddMassTransit(x =>
            {
                var configuration =
                    serviceCollection.BuildServiceProvider()
                        .GetService<IConfiguration>()!;

                x.AddPublishMessageScheduler();

                x.AddHangfireConsumers();

                x.AddConsumer<AbandonChatConsumer>()
                    .Endpoint(e => e.Name = "support-abandon-chat");
                x.AddConsumer<CloseChatConsumer>()
                    .Endpoint(e => e.Name = "support-close-chat");
                x.AddConsumer<MarkMessageAsReadConsumer>()
                    .Endpoint(e => e.Name = "support-mark-message-as-read");
                x.AddConsumer<NewMessageClientNotificationConsumer>()
                    .Endpoint(e => e.Name = "support-new-message-client-notification");
                x.AddConsumer<SendMessageConsumer>()
                    .Endpoint(e => e.Name = "support-send-message");
                x.AddConsumer<SendMessageToChatConsumer>()
                    .Endpoint(e => e.Name = "support-send-message-to-chat");

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.UsePublishMessageScheduler();

                    cfg.Host(configuration["RabbitMQ:Host"], configuration["RabbitMQ:VHost"], h =>
                    {
                        h.Username(configuration["RabbitMQ:Username"]!);
                        h.Password(configuration["RabbitMQ:Password"]!);
                    });

                    cfg.ConfigureEndpoints(context);
                });
            });
        return serviceCollection;
    }
}

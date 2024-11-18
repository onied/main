using MassTransit;
using Users.Services.ProfileProducer;
using Users.Services.UserCreatedProducer;

namespace Users.Extensions;

public static class MassTransitExtensions
{
    public static IServiceCollection AddMassTransitConfigured(this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        serviceCollection.AddMassTransit(x =>
        {
            // A Transport
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
        serviceCollection.AddScoped<IUserCreatedProducer, UserCreatedProducer>();
        serviceCollection.AddScoped<IProfileProducer, ProfileProducer>();

        return serviceCollection;
    }
}

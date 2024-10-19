using Microsoft.Extensions.DependencyInjection;
using Notifications.Data.Abstractions;
using Notifications.Data.Repositories;

namespace Notifications.Data;

public static class BuilderServicesExtension
{
    public static IServiceCollection AddRepositories(this IServiceCollection serviceCollection)
    {
        return serviceCollection.AddScoped<INotificationRepository, NotificationRepository>();
    }
}

using Microsoft.EntityFrameworkCore;
using Notifications.Data;

namespace Notifications.Extensions;

public static class DbContextExtension
{
    public static IServiceCollection AddDbContextConfigured(this IServiceCollection serviceCollection)
    {
        var configuration = serviceCollection.BuildServiceProvider().GetService<IConfiguration>()!;

        serviceCollection.AddDbContext<AppDbContext>(optionsBuilder =>
            optionsBuilder.UseNpgsql(configuration.GetConnectionString("NotificationsDatabase"))
                .UseSnakeCaseNamingConvention());

        return serviceCollection;
    }
}

using Microsoft.EntityFrameworkCore;
using Support.Data;

namespace Support.Extensions;

public static class DbContextExtension
{
    public static IServiceCollection AddDbContextConfigured(this IServiceCollection serviceCollection)
    {
        var configuration = serviceCollection.BuildServiceProvider().GetService<IConfiguration>()!;

        serviceCollection.AddDbContext<AppDbContext>(optionsBuilder =>
            optionsBuilder
                .UseNpgsql(configuration.GetConnectionString("SupportDatabase"))
                .UseSnakeCaseNamingConvention());

        return serviceCollection;
    }
}

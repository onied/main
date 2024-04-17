using Microsoft.EntityFrameworkCore;
using Purchases.Data;

namespace Purchases.Extensions;

public static class DbContextExtension
{
    public static IServiceCollection AddDbContextConfigured(this IServiceCollection serviceCollection)
    {
        var configuration = serviceCollection.BuildServiceProvider().GetService<IConfiguration>()!;

        serviceCollection.AddDbContext<AppDbContext>(optionsBuilder =>
            optionsBuilder.UseNpgsql(configuration.GetConnectionString("PurchasesDatabase"),
                    b => b.MigrationsAssembly("Purchases"))
                .UseSnakeCaseNamingConvention());

        return serviceCollection;
    }
}

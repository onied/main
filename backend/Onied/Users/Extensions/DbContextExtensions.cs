using Microsoft.EntityFrameworkCore;
using Users.Data;

namespace Users.Extensions;

public static class DbContextExtensions
{
    public static IServiceCollection AddDbContextConfigured(this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        serviceCollection.AddDbContext<AppDbContext>(optionsBuilder =>
            optionsBuilder.UseNpgsql(configuration.GetConnectionString("UsersDatabase"))
                .UseSnakeCaseNamingConvention());

        return serviceCollection;
    }
}

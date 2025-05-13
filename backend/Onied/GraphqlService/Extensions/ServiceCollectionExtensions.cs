using Courses.Data;
using Microsoft.EntityFrameworkCore;

namespace GraphqlService.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDbContext(this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        return serviceCollection.AddDbContext<AppDbContext>(optionsBuilder =>
            optionsBuilder.UseNpgsql(configuration.GetConnectionString("CoursesDatabase"))
                .UseSnakeCaseNamingConvention());
    }
}

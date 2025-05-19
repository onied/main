using Courses.Data;
using GraphqlService.Quiries;
using Microsoft.EntityFrameworkCore;

namespace GraphqlService.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDbContext(
        this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        return serviceCollection.AddDbContext<AppDbContext>(optionsBuilder =>
            optionsBuilder.UseNpgsql(configuration.GetConnectionString("CoursesDatabase"))
                .UseSnakeCaseNamingConvention());
    }

    public static IServiceCollection AddGraphQl(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddGraphQLServer()
            .AddQueryType<CourseQuery>()
            .AddPagingArguments()
            .AddProjections()
            .AddFiltering()
            .AddSorting();
        return serviceCollection;
    }
}

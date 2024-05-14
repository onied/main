using Courses.Data;
using Courses.Profiles.Converters;
using Courses.Services;
using Courses.Services.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Courses.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<ICourseManagementService, CourseManagementService>();
        serviceCollection.AddScoped<ICheckTasksService, CheckTasksService>();
        serviceCollection.AddScoped<IUpdateTasksBlockService, UpdateTasksBlockService>();
        serviceCollection.AddScoped<ICheckTaskManagementService, CheckTaskManagementService>();
        serviceCollection.AddScoped<IManualReviewService, ManualReviewService>();
        serviceCollection.AddScoped<INotificationPreparerService, NotificationPreparerService>();
        serviceCollection.AddScoped<IAccountsService, AccountsService>();
        serviceCollection.AddScoped<ICatalogService, CatalogService>();
        serviceCollection.AddScoped<ICourseService, CourseService>();
        serviceCollection.AddScoped<ISubscriptionManagementService, SubscriptionManagementService>();
        serviceCollection.AddScoped<ILandingPageContentService, LandingPageContentService>();
        return serviceCollection.AddScoped<ITeachingService, TeachingService>();
    }

    public static IServiceCollection AddRepositories(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IUserTaskPointsRepository, UserTaskPointsRepository>();
        serviceCollection.AddScoped<IUserRepository, UserRepository>();
        serviceCollection.AddScoped<ICourseRepository, CourseRepository>();
        serviceCollection.AddScoped<IBlockRepository, BlockRepository>();
        serviceCollection.AddScoped<ICategoryRepository, CategoryRepository>();
        serviceCollection.AddScoped<IUserCourseInfoRepository, UserCourseInfoRepository>();
        serviceCollection.AddScoped<IBlockCompletedInfoRepository, BlockCompletedInfoRepository>();
        serviceCollection.AddScoped<IModuleRepository, ModuleRepository>();
        return serviceCollection.AddScoped<IManualReviewTaskUserAnswerRepository, ManualReviewTaskUserAnswerRepository>();
    }

    public static IHttpClientBuilder AddHttpClient(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        return serviceCollection.AddHttpClient("PurchasesServer", config =>
        {
            config.BaseAddress = new Uri(configuration["PurchasesServerApi"]!);
            config.Timeout = new TimeSpan(0, 0, 30);
            config.DefaultRequestHeaders.Clear();
        });
    }

    public static IServiceCollection AddDbContext(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        return serviceCollection.AddDbContext<AppDbContext>(optionsBuilder =>
            optionsBuilder.UseNpgsql(configuration.GetConnectionString("CoursesDatabase"))
                .UseSnakeCaseNamingConvention());
    }

    public static IServiceCollection AddConverters(this IServiceCollection serviceCollection)
    {
        return serviceCollection.AddTransient<UserAnswerToTasksListConverter>();
    }
}

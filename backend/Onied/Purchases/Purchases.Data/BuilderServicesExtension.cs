using Microsoft.Extensions.DependencyInjection;
using Purchases.Data.Abstractions;
using Purchases.Data.Repositories;
using Purchases.Data.UnitOfWork;

namespace Purchases.Data;

public static class BuilderServicesExtension
{
    public static IServiceCollection AddRepositories(this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .AddScoped<IPurchaseUnitOfWork, PurchaseUnitOfWork>()
            .AddScoped<IUserRepository, UserRepository>()
            .AddScoped<ICourseRepository, CourseRepository>()
            .AddScoped<ISubscriptionRepository, SubscriptionRepository>()
            .AddScoped<IPurchaseRepository, PurchaseRepository>()
            .AddScoped<IUserCourseInfoRepository, UserCourseInfoRepository>();
    }
}

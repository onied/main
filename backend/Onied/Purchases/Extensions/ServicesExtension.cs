using Purchases.Services;
using Purchases.Services.Abstractions;

namespace Purchases.Extensions;

public static class ServicesExtension
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        return services
            .AddScoped<IJwtTokenService, JwtTokenService>(
                x => ActivatorUtilities.CreateInstance<JwtTokenService>(
                    x, x.GetService<IConfiguration>()!["JwtSecretKey"]!))
            .AddScoped<IPurchaseTokenService, PurchaseTokenService>()
            .AddScoped<IPurchaseService, PurchaseService>()
            .AddScoped<IPurchaseMakingService, PurchaseMakingService>()
            .AddScoped<IValidatePurchaseService, ValidatePurchaseService>()
            .AddScoped<ISubscriptionManagementService, SubscriptionManagementService>();
    }
}

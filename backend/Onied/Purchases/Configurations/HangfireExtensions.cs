using Hangfire;
using Hangfire.MemoryStorage;
using Purchases.Data.Models;
using Purchases.Services;
using Purchases.Services.Abstractions;

namespace Purchases.Configurations;

public static class HangfireExtensions
{
    public static IServiceCollection AddHangfireWorker(this IServiceCollection serviceCollection)
        => serviceCollection.AddHangfire(x => x.UseMemoryStorage());

    public static IApplicationBuilder UseHangfireWorker(
        this IApplicationBuilder app,
        IConfiguration configuration)
    {
        var hangfireOptions = configuration.GetSection("Hangfire");
        app.UseHangfireDashboard("/worker");

        app.UseHangfireServer();

        RecurringJob.AddOrUpdate<ISubscriptionManagementService>(
            typeof(ISubscriptionManagementService).FullName,
            x => x.UpdateSubscriptionWithAutoRenewal(),
            hangfireOptions["CronUpdateSubscription"],
            TimeZoneInfo.Utc);

        return app;
    }
}

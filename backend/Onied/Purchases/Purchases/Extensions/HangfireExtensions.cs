using Hangfire;
using Hangfire.MemoryStorage;
using Purchases.Services.Abstractions;

namespace Purchases.Extensions;

public static class HangfireExtensions
{
    public static IServiceCollection AddHangfireWorker(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddHangfire(x => x.UseMemoryStorage());
        return serviceCollection.AddHangfireServer();
    }

    public static IApplicationBuilder UseHangfireWorker(
        this IApplicationBuilder app,
        IConfiguration configuration)
    {
        var hangfireOptions = configuration.GetSection("Hangfire");
        app.UseHangfireDashboard("/worker");

        RecurringJob.AddOrUpdate<ISubscriptionManagementService>(
            typeof(ISubscriptionManagementService).FullName,
            x => x.UpdateSubscriptionWithAutoRenewal(),
            hangfireOptions["CronUpdateSubscription"],
            new RecurringJobOptions
            {
                TimeZone = TimeZoneInfo.Utc
            });

        return app;
    }
}

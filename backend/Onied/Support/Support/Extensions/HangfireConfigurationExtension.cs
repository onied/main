using Hangfire;
using Hangfire.MemoryStorage;
using MassTransit;
using Support.Abstractions;
using Support.Services;

namespace Support.Extensions;

public static class HangfireConfigurationExtension
{
    public static IServiceCollection AddHangfireConfigured(this IServiceCollection serviceCollection)
    {
        return serviceCollection.AddHangfire(h =>
        {
            h.UseRecommendedSerializerSettings();
            h.UseMemoryStorage();
        });
    }
}

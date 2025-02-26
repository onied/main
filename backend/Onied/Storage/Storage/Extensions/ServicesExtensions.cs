using Hangfire;
using Hangfire.MemoryStorage;
using Minio;
using StackExchange.Redis;
using Storage.Abstractions;
using Storage.Services;

namespace Storage.Extensions;

public static class ServicesExtensions
{
    private static IConfiguration GetConfiguration(this IServiceCollection services)
        => services.BuildServiceProvider().GetService<IConfiguration>()
           ?? throw new SystemException("IConfiguration instance not found");

    public static IServiceCollection AddMinioConfigured(this IServiceCollection services)
    {
        var minioConfiguration =
            services.GetConfiguration().GetSection("MinIO")
            ?? throw new SystemException("Minio configuration not found");
        return services.AddMinio(configureClient => configureClient
            .WithEndpoint(minioConfiguration["Endpoint"], Convert.ToInt32(minioConfiguration["Port"]))
            .WithCredentials(minioConfiguration["AccessKey"], minioConfiguration["SecretKey"])
            .WithSSL(false)
            .Build());
    }

    public static IServiceCollection AddRedisConfigured(this IServiceCollection services)
    {
        var connectionString =
            services.GetConfiguration().GetConnectionString("Redis")
            ?? throw new SystemException("Redis connection string not found");
        return services
            .AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(connectionString));
    }

    public static IServiceCollection AddHangfire(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHangfire(x => x.UseMemoryStorage());
        services.AddHangfireServer();

        var recurringJobs = services.BuildServiceProvider().GetService<IRecurringJobManager>();
        recurringJobs.AddOrUpdate<ITemporaryStorageCleanerService>(
            typeof(ITemporaryStorageCleanerService).FullName,
            x => x.CleanTemporaryStorage(),
            configuration.GetSection("Hangfire")["CronTemporaryStorageCleaner"],
            new RecurringJobOptions
            {
                TimeZone = TimeZoneInfo.Utc
            });

        return services;
    }
}

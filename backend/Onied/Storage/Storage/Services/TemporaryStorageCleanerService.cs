using StackExchange.Redis;
using Storage.Abstractions;

namespace Storage.Services;

public class TemporaryStorageCleanerService(IConnectionMultiplexer connectionMultiplexer)
    : ITemporaryStorageCleanerService
{
    public async Task CleanTemporaryStorage()
    {
        foreach (var server in connectionMultiplexer.GetServers())
        {
            await server.FlushAllDatabasesAsync();
        }
    }
}

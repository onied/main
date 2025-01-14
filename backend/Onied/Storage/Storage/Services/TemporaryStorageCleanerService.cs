using StackExchange.Redis;
using Storage.Abstractions;

namespace Storage.Services;

public class TemporaryStorageCleanerService(IServer server) : ITemporaryStorageCleanerService
{
    public async Task CleanTemporaryStorage()
    {
        await server.FlushAllDatabasesAsync();
    }
}

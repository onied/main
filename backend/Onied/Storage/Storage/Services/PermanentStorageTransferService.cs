using CommunityToolkit.HighPerformance.Helpers;
using StackExchange.Redis;
using Storage.Abstractions;

namespace Storage.Services;

public class PermanentStorageTransferService(IDatabaseAsync db, ILogger<PermanentStorageTransferService> logger)
    : IPermanentStorageTransferService
{
    private const string CounterField = "counter";
    private const string MetadataField = "metadata";

    public async Task TransferAfterUpload(string fileId)
    {
        var uploadExists = await db.KeyExistsAsync(fileId);
        if (!uploadExists || !await db.HashExistsAsync(fileId, CounterField))
        {
            var succeeded = await db.HashSetAsync(fileId, CounterField, 0);
            if (!succeeded)
            {
                logger.LogError("Could not create key with counter in redis: {key}", fileId);
                return;
            }
        }

        var counter = await db.HashGetAsync(fileId, CounterField);
        var parsed = counter.TryUnbox(out int counterValue);
        if (!parsed)
        {
            logger.LogError("Could not parse counter as int: {counter}", counter);
            return;
        }

        if (counterValue < 2)
        {
            await db.HashIncrementAsync(fileId, CounterField);
            return;
        }
    }
}

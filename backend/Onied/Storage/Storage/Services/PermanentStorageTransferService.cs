using CommunityToolkit.HighPerformance.Helpers;
using Minio;
using Minio.DataModel.Args;
using StackExchange.Redis;
using Storage.Abstractions;

namespace Storage.Services;

public class PermanentStorageTransferService(
    IDatabaseAsync db,
    IMinioClient minioClient,
    ILogger<PermanentStorageTransferService> logger)
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

        var redisHashObject = await db.HashGetAllAsync(fileId);
        var counter = redisHashObject.FirstOrDefault(entry => entry.Name == CounterField).Value;
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

        var metadata = redisHashObject.FirstOrDefault(entry => entry.Name == MetadataField).Value;
        var metadataString = metadata.ToString();

        var copySourceObjectArgs = new CopySourceObjectArgs()
            .WithBucket(Constants.Buckets.Temporary)
            .WithObject(fileId);
        var copyObjectArgs = new CopyObjectArgs()
            .WithBucket(Constants.Buckets.Permanent)
            .WithObject(fileId)
            .WithCopyObjectSource(copySourceObjectArgs)
            .WithHeaders(new Dictionary<string, string>() { { "metadata", metadataString } });
        await minioClient.CopyObjectAsync(copyObjectArgs);

        var removeArgs = new RemoveObjectArgs()
            .WithBucket(Constants.Buckets.Temporary)
            .WithObject(fileId);
        await minioClient.RemoveObjectAsync(removeArgs);
    }
}

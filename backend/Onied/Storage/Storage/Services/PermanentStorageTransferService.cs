using CommunityToolkit.HighPerformance.Helpers;
using Minio;
using Minio.DataModel;
using Minio.DataModel.Args;
using StackExchange.Redis;
using Storage.Abstractions;

namespace Storage.Services;

public class PermanentStorageTransferService(
    IRedisRepository redisRepository,
    IMinioClient minioClient,
    ILogger<PermanentStorageTransferService> logger)
    : IPermanentStorageTransferService
{
    private string GetHashSetKey(string id) => $"temporary_files__{id}";
    private static readonly string Counter = nameof(Counter).ToLowerInvariant();
    private static readonly string Metadata = nameof(Metadata).ToLowerInvariant();

    public async Task TransferAfterUpload(string fileId)
    {
        var counter = await redisRepository.GetHashSetValue<int?>(GetHashSetKey(fileId), Counter);
        var metadata = await redisRepository.GetHashSetValue<string?>(GetHashSetKey(fileId), Metadata);

        if (counter == null)
        {
            logger.LogError("No counter at fileId: {fileId}", fileId);
            return;
        }

        if (counter < 2 || metadata == null)
        {
            logger.LogInformation("Not enough uploaded: {counter}", counter);
            return;
        }

        logger.LogInformation("Starting uploading file {fileId} to permanent storage with metadata {metadata}...",
            fileId, metadata);

        var bucketExistsArgs = new BucketExistsArgs().WithBucket(Constants.Buckets.Permanent);
        var bucketExists = await minioClient.BucketExistsAsync(bucketExistsArgs);
        if (!bucketExists)
        {
            var makeBucketArgs = new MakeBucketArgs().WithBucket(Constants.Buckets.Permanent);
            await minioClient.MakeBucketAsync(makeBucketArgs);
        }

        var statObjectArgs = new StatObjectArgs()
            .WithBucket(Constants.Buckets.Temporary)
            .WithObject(fileId);
        var stat = await minioClient.StatObjectAsync(statObjectArgs);

        var copyConditions = new CopyConditions();
        copyConditions.SetReplaceMetadataDirective();
        stat.MetaData.Add("Custom-Metadata", metadata);
        var copySourceObjectArgs = new CopySourceObjectArgs()
            .WithBucket(Constants.Buckets.Temporary)
            .WithObject(fileId)
            .WithCopyConditions(copyConditions);
        var copyObjectArgs = new CopyObjectArgs()
            .WithBucket(Constants.Buckets.Permanent)
            .WithObject(fileId)
            .WithCopyObjectSource(copySourceObjectArgs)
            .WithHeaders(stat.MetaData);
        await minioClient.CopyObjectAsync(copyObjectArgs);


        logger.LogInformation("Uploaded file {fileId} to permanent storage", fileId);

        var removeArgs = new RemoveObjectArgs()
            .WithBucket(Constants.Buckets.Temporary)
            .WithObject(fileId);
        await minioClient.RemoveObjectAsync(removeArgs);

        logger.LogInformation("Removed file {fileId} from temporary storage", fileId);
    }
}

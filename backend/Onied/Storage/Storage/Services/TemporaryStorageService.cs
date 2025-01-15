using System.Net;
using System.Text.Json;
using System.Text.RegularExpressions;
using Minio;
using Minio.DataModel.Args;
using Minio.Exceptions;
using Storage.Abstractions;
using Storage.Exceptions;
using Unidecode.NET;

namespace Storage.Services;

public class TemporaryStorageService(
    IMinioClient minio,
    IConfiguration configuration,
    ILogger<TemporaryStorageService> logger)
{
    private string GetHashSetKey(Guid id) => $"temporary_files__{id}";
    private static readonly string Counter = nameof(Counter).ToLowerInvariant();
    private static readonly string Metadata = nameof(Metadata).ToLowerInvariant();

    private static readonly SemaphoreSlim SemaphoreSlim = new(1, 1);

    private async Task UploadFileToMinio(IFormFile file, Guid fileId)
    {
        try
        {
            var minioConfiguration = configuration.GetSection("MinIO");
            var bucketName = minioConfiguration["TemporaryBucket"];

            var beArgs = new BucketExistsArgs().WithBucket(bucketName);
            var found = await minio.BucketExistsAsync(beArgs);
            if (!found)
            {
                var mbArgs = new MakeBucketArgs()
                    .WithBucket(bucketName);
                await minio.MakeBucketAsync(mbArgs).ConfigureAwait(false);
            }

            var fileWrapper = new
            {
                File = file,
                DownloadName = new string(file.FileName
                    .Unidecode()
                    .ToLowerInvariant()
                    .Select(letter => Regex.IsMatch([letter], @"^[0-9a-z\-\.]$") ? letter : '_')
                    .TakeLast(64)
                    .ToArray()),
                ObjectName = fileId.ToString()
            };

            var putObjectArgs = new PutObjectArgs()
                .WithBucket(bucketName)
                .WithObject(fileWrapper.ObjectName)
                .WithContentType(fileWrapper.File.ContentType)
                .WithObjectSize(fileWrapper.File.Length)
                .WithStreamData(fileWrapper.File.OpenReadStream())
                .WithHeaders(new Dictionary<string, string> { { "x-amz-meta-download-name", fileWrapper.DownloadName } });
            await minio.PutObjectAsync(putObjectArgs).ConfigureAwait(false);
            logger.LogInformation("Uploaded temporary file id={fileId}", fileId);
        }
        catch (MinioException e)
        {
            logger.LogError("Could not upload files. Reason: {message}", e.Message);
            throw new HttpResponseException("Could not upload files.", HttpStatusCode.InternalServerError);
        }
    }

    public async Task<IResult> InitUpload(IRedisRepository redisRepository)
    {
        var id = Guid.NewGuid();
        await redisRepository.SetHashSetValue(GetHashSetKey(id), Counter, 0);

        logger.LogInformation("Initialized temporary file id={id} upload", id);
        return Results.Ok(id);
    }

    public async Task<IResult> UploadFile(Guid fileId,
       IFormFile file,
        IRedisRepository redisRepository)
    {
        var counter = await redisRepository.GetHashSetValue<int?>(GetHashSetKey(fileId), Counter);

        if (counter is null)
        {
            logger.LogError($"File with id={fileId} not found");
            throw new NotFoundException($"File with id={fileId} not found");
        }
        if (counter is < 2)
        {
            try
            {
                await SemaphoreSlim.WaitAsync();
                counter = await redisRepository.GetHashSetValue<int?>(GetHashSetKey(fileId), Counter);
                if (counter is < 2)
                {
                    await UploadFileToMinio(file, fileId);
                    await redisRepository.SetHashSetValue(GetHashSetKey(fileId), Counter, counter + 1);
                }
            }
            finally
            {
                SemaphoreSlim.Release();
            }
        }
        // кинуть ошибку при переполнении

        return Results.Ok();
    }
    public async Task<IResult> UploadMetadata(
        Guid fileId,
        JsonElement jsonText,
        IRedisRepository redisRepository)
    {
        var rawString = jsonText.GetRawText();
        var counter = await redisRepository.GetHashSetValue<int?>(GetHashSetKey(fileId), Counter);

        if (counter is null)
            throw new NotFoundException($"File with id={fileId} not found");
        if (counter is < 2)
        {
            try
            {
                await SemaphoreSlim.WaitAsync();
                counter = await redisRepository.GetHashSetValue<int?>(GetHashSetKey(fileId), Counter);
                if (counter is < 2)
                {
                    await redisRepository.SetHashSetValue(GetHashSetKey(fileId), Metadata, rawString);
                    logger.LogInformation("Uploaded temporary file metadata id={fileId}", fileId);
                    await redisRepository.SetHashSetValue(GetHashSetKey(fileId), Counter, counter + 1);
                }
            }
            finally
            {
                SemaphoreSlim.Release();
            }
        }
        // кинуть ошибку при переполнении

        return Results.Ok();
    }
}

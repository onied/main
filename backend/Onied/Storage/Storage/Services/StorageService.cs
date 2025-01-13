using System.Net;
using System.Text.RegularExpressions;
using Minio;
using Minio.DataModel.Args;
using Minio.Exceptions;
using Storage.Abstractions;
using Storage.Constants;
using Storage.Exceptions;
using Unidecode.NET;

namespace Storage.Services;

public class StorageService(
    IMinioClient minio,
    IConfiguration configuration,
    ILogger<StorageService> logger) : IStorageService
{
    public async Task<IResult> Upload(IFormFileCollection files)
    {
        if (files.Count > 10)
            throw new BadRequestException("Too many files");
        if (files.Any(file => file.Length > 5 * 1024 * 1024))
            throw new BadRequestException("Some files are too large");
        var objects = files.Select(file =>
            new
            {
                File = file,
                DownloadName = new string(file.FileName
                    .Unidecode()
                    .ToLowerInvariant()
                    .Select(letter => Regex.IsMatch([letter], @"^[0-9a-z\-\.]$") ? letter : '_')
                    .TakeLast(64)
                    .ToArray()),
                ObjectName = string.Concat(
                    new string(file.FileName
                        .Unidecode()
                        .ToLowerInvariant()
                        .Select(letter => Regex.IsMatch([letter], @"^[0-9a-z\-]$") ? letter : '_')
                        .Take(27)
                        .ToArray()),
                    "-",
                    Guid.NewGuid())
            }).ToArray();

        try
        {
            var minioConfiguration = configuration.GetSection("MinIO");
            var bucketName = minioConfiguration["Bucket"];

            // Make a bucket on the server, if not already present.
            var beArgs = new BucketExistsArgs().WithBucket(bucketName);
            var found = await minio.BucketExistsAsync(beArgs);
            if (!found)
            {
                var mbArgs = new MakeBucketArgs()
                    .WithBucket(bucketName);
                await minio.MakeBucketAsync(mbArgs).ConfigureAwait(false);
            }

            foreach (var obj in objects)
            {
                // Upload a file to bucket.
                var putObjectArgs = new PutObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(obj.ObjectName)
                    .WithContentType(obj.File.ContentType)
                    .WithObjectSize(obj.File.Length)
                    .WithStreamData(obj.File.OpenReadStream())
                    .WithHeaders(new Dictionary<string, string> { { "x-amz-meta-download-name", obj.DownloadName } });
                await minio.PutObjectAsync(putObjectArgs).ConfigureAwait(false);
                logger.LogInformation("Uploaded file: {filename}", obj.ObjectName);
            }
        }
        catch (MinioException e)
        {
            logger.LogError("Could not upload files. Reason: {message}", e.Message);
            throw new HttpResponseException("Could not upload files.", HttpStatusCode.InternalServerError);
        }

        return TypedResults.Ok(
            objects.Select(obj =>
                new
                {
                    objectName = obj.ObjectName,
                    filename = obj.DownloadName
                }));
    }

    public async Task<IResult> Get(string objectName)
    {
        var minioConfiguration = configuration.GetSection("MinIO");
        try
        {
            var bucketName = minioConfiguration["Bucket"];
            var objectStatArgs = new StatObjectArgs()
                .WithBucket(bucketName)
                .WithObject(objectName);
            var obj = await minio.StatObjectAsync(objectStatArgs).ConfigureAwait(false);
            obj.MetaData.TryGetValue("download-name", out var downloadName);
            var args = new PresignedGetObjectArgs()
                .WithBucket(bucketName)
                .WithObject(objectName)
                .WithHeaders(new Dictionary<string, string>()
                    { { "response-content-disposition", $"attachment; filename=\"{downloadName}\"" } })
                .WithExpiry(30);
            var presignedUrl = await minio.PresignedGetObjectAsync(args).ConfigureAwait(false);
            return TypedResults.Ok(new { presignedUrl });
        }
        catch (Exception e) when (e is BucketNotFoundException or ObjectNotFoundException)
        {
            throw new NotFoundException("Could not find file.");
        }
        catch (MinioException e)
        {
            logger.LogError("Could not get a file. Reason: {message}", e.Message);
            throw new HttpResponseException("Could not get a file.", HttpStatusCode.InternalServerError);
        }
    }

    public async Task<IResult> GetMetadata(string fileId)
    {
        try
        {
            var objectStatArgs = new StatObjectArgs()
                .WithBucket(Buckets.Permanent)
                .WithObject(fileId);
            var obj = await minio.StatObjectAsync(objectStatArgs).ConfigureAwait(false);

            var metadata = new Dictionary<string, string>();
            foreach (var (key, value) in obj.MetaData)
            {
                if (key.StartsWith("x-amz-meta-"))
                {
                    metadata[key] = value;
                }
            }

            return TypedResults.Ok(metadata);
        }
        catch (Exception e) when (e is BucketNotFoundException or ObjectNotFoundException)
        {
            throw new NotFoundException("Could not find file metadata.");
        }
        catch (MinioException e)
        {
            logger.LogError("Could not get file metadata. Reason: {message}", e.Message);
            throw new HttpResponseException("Could not get file metadata.", HttpStatusCode.InternalServerError);
        }
    }

    public async Task<IResult> GetUrlByFileId(string fileId)
    {
        try
        {
            var objectStatArgs = new StatObjectArgs()
                .WithBucket(Buckets.Permanent)
                .WithObject(fileId);
            var obj = await minio.StatObjectAsync(objectStatArgs).ConfigureAwait(false);

            var args = new PresignedGetObjectArgs()
                .WithBucket(Buckets.Permanent)
                .WithObject(fileId)
                .WithHeaders(new Dictionary<string, string>
                    { { "response-content-disposition", $"attachment; filename=\"{obj.MetaData["download-name"]}\"" } })
                .WithExpiry(3600);
            var presignedUrl = await minio.PresignedGetObjectAsync(args).ConfigureAwait(false);

            return TypedResults.Ok(new { presignedUrl });
        }
        catch (Exception e) when (e is BucketNotFoundException or ObjectNotFoundException)
        {
            throw new NotFoundException("Could not find file.");
        }
        catch (MinioException e)
        {
            logger.LogError("Could not generate download URL. Reason: {message}", e.Message);
            throw new HttpResponseException("Could not generate download URL.", HttpStatusCode.InternalServerError);
        }
    }
}

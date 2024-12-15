using System.Net;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Minio;
using Minio.DataModel.Args;
using Minio.Exceptions;
using Storage.Exceptions;
using Unidecode.NET;

namespace Storage.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class StorageController(
    LinkGenerator linkGenerator,
    IMinioClient minio,
    IConfiguration configuration,
    ILogger<StorageController> logger)
    : ControllerBase
{
    [HttpPost]
    [Route("upload")]
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
                linkGenerator.GetUriByAction(HttpContext, nameof(Get), values: new { filename = obj.ObjectName })));
    }

    [HttpGet]
    [Route("get/{filename}")]
    public async Task<IResult> Get(string filename)
    {
        var minioConfiguration = configuration.GetSection("MinIO");
        try
        {
            var stream = new MemoryStream();
            var bucketName = minioConfiguration["Bucket"];
            var args = new GetObjectArgs()
                .WithBucket(bucketName)
                .WithObject(filename)
                .WithCallbackStream(async (s, token) => await s.CopyToAsync(stream, token));
            var file = await minio.GetObjectAsync(args).ConfigureAwait(false);
            file.MetaData.TryGetValue("download-name", out var downloadName);
            logger.LogInformation("Got file stream: {filename}, filename: {downloadName} (size: {size})", filename,
                downloadName, file.Size);
            stream.Seek(0, SeekOrigin.Begin);
            return TypedResults.File(stream, file.ContentType, downloadName, file.LastModified);
        }
        catch (Exception e) when (e is BucketNotFoundException or ObjectNotFoundException)
        {
            throw new NotFoundException("Could not find file.");
        }
        catch (MinioException e)
        {
            logger.LogError("Could not download a file. Reason: {message}", e.Message);
            throw new HttpResponseException("Could not download a file.", HttpStatusCode.InternalServerError);
        }
    }
}

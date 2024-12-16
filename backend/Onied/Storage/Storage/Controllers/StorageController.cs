using System.Net;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Minio;
using Minio.DataModel.Args;
using Minio.Exceptions;
using Storage.Abstractions;
using Storage.Exceptions;
using Unidecode.NET;

namespace Storage.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class StorageController(IStorageService storageService)
    : ControllerBase
{
    [HttpPost]
    [Route("upload")]
    public async Task<IResult> Upload(IFormFileCollection files)
    {
        return await storageService.Upload(files);
    }

    [HttpGet]
    [Route("get/{objectName}")]
    public async Task<IResult> Get(string objectName)
    {
        return await storageService.Get(objectName);
    }
}

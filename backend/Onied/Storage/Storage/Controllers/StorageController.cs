using System.Net;
using System.Text.RegularExpressions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Minio;
using Minio.DataModel.Args;
using Minio.Exceptions;
using Storage.Abstractions;
using Storage.Commands;
using Storage.Exceptions;
using Storage.Queries;
using Unidecode.NET;

namespace Storage.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class StorageController(ISender sender)
    : ControllerBase
{
    [HttpPost]
    [Route("upload")]
    public async Task<IResult> Upload(IFormFileCollection files)
    {
        return await sender.Send(new Upload(files));
    }

    [HttpGet]
    [Route("get/{objectName}")]
    public async Task<IResult> Get(string objectName)
    {
        return await sender.Send(new Get(objectName));
    }
}

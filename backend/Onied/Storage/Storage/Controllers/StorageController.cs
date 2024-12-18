using MediatR;
using Microsoft.AspNetCore.Mvc;
using Storage.Commands;
using Storage.Queries;

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

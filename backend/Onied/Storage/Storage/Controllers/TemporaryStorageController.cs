using System.Text.Json;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Storage.Commands;

namespace Storage.Controllers;

[ApiController]
[Route("temporary-storage")]
public class TemporaryStorageController(ISender sender)
{
    [HttpPost("init")]
    public async Task<IResult> InitUpload()
    {
        return await sender.Send(new InitUploadTemporaryFile());
    }

    [HttpPost("{fileId:guid}/file")]
    public async Task<IResult> UploadFile(Guid fileId, IFormFile file)
    {
        return await sender.Send(new UploadTemporaryFile(fileId, file));
    }

    [HttpPost("{fileId:guid}/metadata")]
    public async Task<IResult> UploadMetadata(Guid fileId, [FromBody] JsonElement rawJsonText)
    {
        return await sender.Send(new UploadMetadata(fileId, rawJsonText));
    }
}

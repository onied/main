using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Mvc;

namespace Storage.Controllers;

[ApiController]
[Route("[controller]")]
public class TemporaryStorageController
{
    [HttpPost("{fileId:guid}/file")]
    public Task<IResult> LoadFile(Guid fileId, [FromForm] IFormFile file)
    {
        throw new NotImplementedException();
    }

    [HttpPost("{fileId:guid}/metadata")]
    public Task<IResult> LoadMetadata(Guid fileId, [FromBody] JsonObject metadata)
    {
        return Task.FromResult(Results.Ok(metadata));
    }
}

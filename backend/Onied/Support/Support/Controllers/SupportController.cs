using Microsoft.AspNetCore.Mvc;

namespace Support.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class SupportController : ControllerBase
{
    [HttpGet]
    [Route("active")]
    public Task<IResult> GetActiveChats([FromQuery] Guid? userId)
    {
        return Task.FromResult(Results.Ok(new { UserId = userId.ToString() }));
    }

    [HttpGet]
    [Route("open")]
    public Task<IResult> GetOpenChats([FromQuery] Guid? userId)
    {
        return Task.FromResult(Results.Ok(new { UserId = userId.ToString() }));
    }
}

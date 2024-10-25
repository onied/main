using Microsoft.AspNetCore.Mvc;
using Support.Abstractions;

namespace Support.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class SupportController(ISupportService supportService) : ControllerBase
{
    [HttpGet]
    [Route("active")]
    public async Task<IResult> GetActiveChats([FromQuery] Guid? userId)
    {
        var response = await supportService.GetActiveChats(userId);
        return Results.Ok(response);
    }

    [HttpGet]
    [Route("open")]
    public async Task<IResult> GetOpenChats([FromQuery] Guid? userId)
    {
        var response = await supportService.GetOpenChats(userId);
        return Results.Ok(response);
    }

    [HttpGet]
    [Route("profile")]
    public async Task<IResult> GetProfile([FromQuery] Guid? userId)
    {
        var response = await supportService.GetProfile(userId);
        return Results.Ok(response);
    }
}

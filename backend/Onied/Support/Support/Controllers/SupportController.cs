using MediatR;
using Microsoft.AspNetCore.Mvc;
using Support.Authorization.Filters;
using Support.Queries;

namespace Support.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[AuthorizeSupportUser]
public class SupportController(ISender sender) : ControllerBase
{
    [HttpGet]
    [Route("active")]
    public async Task<IResult> GetActiveChats([FromQuery] Guid? userId)
        => await sender.Send(new GetActiveChatsQuery(userId));

    [HttpGet]
    [Route("open")]
    public async Task<IResult> GetOpenChats([FromQuery] Guid? userId)
        => await sender.Send(new GetOpenChatsQuery(userId));

    [HttpGet]
    [Route("profile")]
    public async Task<IResult> GetProfile([FromQuery] Guid? userId)
        => await sender.Send(new GetProfileQuery(userId));
}

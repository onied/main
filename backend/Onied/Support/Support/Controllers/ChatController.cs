using MediatR;
using Microsoft.AspNetCore.Mvc;
using Support.Authorization.Filters;
using Support.Queries;

namespace Support.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ChatController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<IResult> GetUserChat(
        [FromQuery] Guid? userId)
        => await sender.Send(new GetUserChatQuery(userId));

    [HttpGet]
    [Route("{chatId:guid}")]
    [AuthorizeSupportUser]
    public async Task<IResult> GetChatById([FromRoute] Guid chatId, [FromQuery] Guid? userId)
        => await sender.Send(new GetChatByIdQuery(chatId, userId));
}

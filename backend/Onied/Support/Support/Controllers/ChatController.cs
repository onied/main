using Microsoft.AspNetCore.Mvc;

namespace Support.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ChatController : ControllerBase
{
    [HttpGet]
    public Task<IResult> GetUserChat(
        [FromQuery] Guid? userId)
    {
        return Task.FromResult(Results.Ok(new { UserId = userId.ToString() }));
    }

    [HttpGet]
    [Route("{chatId:guid}")]
    public Task<IResult> GetChatById([FromRoute] Guid chatId, [FromQuery] Guid? userId)
    {
        return Task.FromResult(Results.Ok(new { ChatId = chatId.ToString(), UserId = userId.ToString() }));
    }
}

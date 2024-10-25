using Microsoft.AspNetCore.Mvc;
using Support.Abstractions;

namespace Support.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ChatController(IChatService chatService) : ControllerBase
{
    [HttpGet]
    public async Task<IResult> GetUserChat(
        [FromQuery] Guid? userId)
    {
        var response = await chatService.GetUserChat(userId);
        return Results.Ok(response);
    }

    [HttpGet]
    [Route("{chatId:guid}")]
    public async Task<IResult> GetChatById([FromRoute] Guid chatId, [FromQuery] Guid? userId)
    {
        var response = await chatService.GetChatById(chatId, userId);
        return Results.Ok(response);
    }
}

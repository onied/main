using MediatR;
using Microsoft.AspNetCore.Mvc;
using Notifications.Queries;

namespace Notifications.Controllers;

[Route("/api/v1/[controller]")]
public class NotificationsController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<IResult> Get([FromQuery] Guid userId)
        => await sender.Send(new GetMessagesQuery(userId));
}

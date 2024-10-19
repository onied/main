using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Notifications.Data.Abstractions;
using Notifications.Dtos;

namespace Notifications.Controllers;

[Route("/api/v1/[controller]")]
public class NotificationsController(
    IMapper mapper,
    INotificationRepository notificationRepository
    ) : ControllerBase
{
    [HttpGet]
    public async Task<IResult> Get([FromQuery] Guid userId)
    {
        var notifications = await notificationRepository.GetRangeByUserAsync(userId);
        return Results.Ok(mapper.Map<List<NotificationDto>>(notifications));
    }
}

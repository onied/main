using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Notifications.Data.Abstractions;
using Notifications.Dtos.Responses;

namespace Notifications.Controllers;

[Route("/api/v1/[controller]")]
public class NotificationsController(
    IMapper mapper,
    INotificationRepository notificationRepository
    ) : ControllerBase
{
    public async Task<IResult> Get([FromQuery] Guid userId)
    {
        var notifications = await notificationRepository.GetRangeByUserAsync(userId);
        return Results.Ok(mapper.Map<List<NotificationResponseDto>>(notifications));
    }
}

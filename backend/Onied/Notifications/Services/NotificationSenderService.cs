using AutoMapper;
using MassTransit.Data.Messages;
using Microsoft.AspNetCore.SignalR;
using Notifications.Data.Abstractions;
using Notifications.Data.Models;
using Notifications.Dtos.Responses;
using Notifications.Hubs;
using Notifications.Services.Abstractions;

namespace Notifications.Services;

public class NotificationSenderService(
        IMapper mapper,
        INotificationRepository notificationRepository,
        IHubContext<NotificationsHub> hubContext
    ) : INotificationSenderService
{
    public async Task Send(NotificationSent notificationSent)
    {
        var notification = mapper.Map<Notification>(notificationSent);
        notification = await notificationRepository.AddAsync(notification);

        var dto = mapper.Map<NotificationDto>(notification);
        await hubContext.Clients.User(notification.UserId.ToString()).SendAsync("Receive", dto);
    }
}

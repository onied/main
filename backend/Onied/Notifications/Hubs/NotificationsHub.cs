using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Notifications.Data.Abstractions;
using Notifications.Data.Models;
using Notifications.Dtos.Responses;

namespace Notifications.Hubs;

public class NotificationsHub(
    IMapper mapper,
    INotificationRepository notificationRepository
    ) : Hub
{
    public override Task OnConnectedAsync()
    {
        Console.WriteLine("A Client Connected: " + Context.ConnectionId);
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        Console.WriteLine("A client disconnected: " + Context.ConnectionId);
        return base.OnDisconnectedAsync(exception);
    }

    public async Task Send(Notification notification)
    {
        var storedNotification = await notificationRepository.AddAsync(notification);
        var dto = mapper.Map<NotificationResponseDto>(storedNotification);
        await Clients.Client(notification.UserId.ToString()).SendAsync("Receive", dto);
    }
}

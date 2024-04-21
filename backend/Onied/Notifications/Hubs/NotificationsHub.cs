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

    public async Task UpdateRead(int id)
    {
        Console.WriteLine(Context.UserIdentifier);
        var notification = await notificationRepository.GetAsync(id);
        if (notification is null) return;

        notification.IsRead = true;
        await notificationRepository.UpdateAsync(notification);
    }

    public async Task Send(Notification notification)
    {
        var storedNotification = await notificationRepository.AddAsync(notification);
        var dto = mapper.Map<NotificationDto>(storedNotification);
        await Clients.User(notification.UserId.ToString()).SendAsync("Receive", dto);
    }
}

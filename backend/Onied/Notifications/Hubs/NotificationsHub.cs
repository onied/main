using Microsoft.AspNetCore.SignalR;
using Notifications.Data.Abstractions;

namespace Notifications.Hubs;

public class NotificationsHub(INotificationRepository notificationRepository) : Hub
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
        var notification = await notificationRepository.GetAsync(id);
        if (notification is null) return;

        notification.IsRead = true;
        await notificationRepository.UpdateAsync(notification);
    }
}

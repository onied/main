using Microsoft.AspNetCore.SignalR;

namespace Notifications.Hubs;

public class NotificationsHub : Hub
{
    /*public override Task OnConnectedAsync()
    {
        Console.WriteLine("A Client Connected: " + Context.ConnectionId);
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception exception)
    {
        Console.WriteLine("A client disconnected: " + Context.ConnectionId);
        return base.OnDisconnectedAsync(exception);
    }
    
    public async Task Send(Message message)
    {
        Console.WriteLine($"message: {message.Text}");
        await Clients.All.SendAsync("Receive", message);
        await messageService.AddMessage(message);
        // await Clients.All.SendAsync("Receive", message);
    }*/
}

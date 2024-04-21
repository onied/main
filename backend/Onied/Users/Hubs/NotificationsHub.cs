using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Users.Dtos;

namespace Users.Hubs;

[Authorize]
public class NotificationsHub(IConfiguration configuration) : Hub
{
    private HubConnection _realHubConnection;

    public override Task OnConnectedAsync()
    {
        Console.WriteLine("A Client Connected: " + Context.ConnectionId);

        _realHubConnection = new HubConnectionBuilder()
            .WithUrl(configuration["ConfigWsServer"]! + $"?userId={Context.UserIdentifier}")
            .Build();
        _realHubConnection.StartAsync();

        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        _realHubConnection?.StopAsync();

        Console.WriteLine("A client disconnected: " + Context.ConnectionId);
        return base.OnDisconnectedAsync(exception);
    }

    public async Task UpdateRead(int id)
        => await _realHubConnection.SendAsync("UpdateRead", id);

    public async Task Receive(NotificationDto dto)
        => await Clients
            .User(Context.UserIdentifier ?? Guid.Empty.ToString())
            .SendAsync("Receive", dto);
}

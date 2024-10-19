using Microsoft.AspNetCore.SignalR;

namespace Notifications.Services;

public class MyCustomProvider : IUserIdProvider
{
    public string GetUserId(HubConnectionContext connection)
    {
        return connection.GetHttpContext()?.Request.Query["userId"]!;
    }
}

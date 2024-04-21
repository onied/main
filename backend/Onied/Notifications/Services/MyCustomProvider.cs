using Microsoft.AspNetCore.SignalR;

namespace Notifications.Services;

public class MyCustomProvider : IUserIdProvider
{
    public string GetUserId(HubConnectionContext connection)
    {
        Console.WriteLine(connection.GetHttpContext()?.Request.QueryString);
        return connection.GetHttpContext()?.Request.Query["userId"]!;
    }
}

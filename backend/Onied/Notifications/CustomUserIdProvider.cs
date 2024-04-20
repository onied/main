using Microsoft.AspNetCore.SignalR;

namespace Notifications;

public class MyCustomProvider : IUserIdProvider
{
    public string GetUserId(HubConnectionContext connection)
        => (connection.GetHttpContext()?.Request.Query["userId"] ?? Guid.Empty.ToString())!;
}

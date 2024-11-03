using Microsoft.AspNetCore.SignalR;

namespace Support.Services;

public class QueryUserIdProvider : IUserIdProvider
{
    public string GetUserId(HubConnectionContext connection)
    {
        return connection.GetHttpContext()?.Request.Query["userId"]!;
    }
}

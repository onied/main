using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Support.Abstractions;
using Support.Authorization.Requirements;

namespace Support.Hubs;

public class ChatHub : Hub<IChatClient>
{
    private const string SupportUserGroup = "SupportUsers";
    public const string IsSupportUserItem = "IsSupportUser";

    public override async Task OnConnectedAsync()
    {
        // TODO: check if user is in Support db
        var isSupportUser = false;
        if (isSupportUser)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, SupportUserGroup);
            Context.Items[IsSupportUserItem] = true;
        }
        await base.OnConnectedAsync();
    }

    public Task SendMessage(string messageContent)
    {
        // TODO: If currentSessionId == null send "Receive" to group operators
        throw new NotImplementedException();
    }

    public Task MarkMessageAsRead(Guid messageId)
    {
        throw new NotImplementedException();
    }

    [Authorize(SupportUserRequirement.Policy)]
    public Task SendMessageToChat(Guid chatId, string messageContent)
    {
        // TODO: If new session established, send "RemoveChatFromOpened" to group operators
        throw new NotImplementedException();
    }

    [Authorize(SupportUserRequirement.Policy)]
    public Task MarkMessageAsReadInChat(Guid chatId, Guid messageId)
    {
        throw new NotImplementedException();
    }

    [Authorize(SupportUserRequirement.Policy)]
    public Task CloseChat(Guid chatId)
    {
        // TODO: If currentSessionId == null send "RemoveChatFromOpened" to group operators
        throw new NotImplementedException();
    }

    [Authorize(SupportUserRequirement.Policy)]
    public Task AbandonChat(Guid chatId)
    {
        // TODO: Resend last message to group operators
        throw new NotImplementedException();
    }
}

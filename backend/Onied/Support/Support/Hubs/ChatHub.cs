using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Support.Abstractions;
using Support.Authorization.Requirements;
using Support.Data.Abstractions;
using Support.Events.Messages;
using Support.Helpers;

namespace Support.Hubs;

public class ChatHub(
    ILogger<ChatHub> logger,
    ISupportUserRepository supportUserRepository,
    IPublishEndpoint publishEndpoint)
    : Hub<IChatClient>
{
    public const string SupportUserGroup = "SupportUsers";

    public override async Task OnConnectedAsync()
    {
        var parsed = Guid.TryParse(Context.UserIdentifier, out var userId);
        if (!parsed)
        {
            logger.LogError("Could not parse user identifier: {id}", Context.UserIdentifier);
            Context.Abort();
            return;
        }

        var items = new ChatHubContextItemsHelper(Context.Items)
        {
            UserId = userId
        };

        var supportUser = await supportUserRepository.GetAsync(userId);
        if (supportUser != null)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, SupportUserGroup);
            items.IsSupportUser = true;
        }
    }

    public async Task SendMessage(string messageContent)
    {
        await publishEndpoint.Publish(
            new SendMessage(new ChatHubContextItemsHelper(Context.Items).UserId, messageContent));
    }

    public async Task MarkMessageAsRead(Guid messageId)
    {
        await publishEndpoint.Publish(
            new MarkMessageAsRead(new ChatHubContextItemsHelper(Context.Items).UserId, messageId));
    }

    [Authorize(SupportUserRequirement.Policy)]
    public async Task SendMessageToChat(Guid chatId, string messageContent)
    {
        await publishEndpoint.Publish(
            new SendMessageToChat(new ChatHubContextItemsHelper(Context.Items).UserId, chatId, messageContent));
    }

    [Authorize(SupportUserRequirement.Policy)]
    public async Task CloseChat(Guid chatId)
    {
        await publishEndpoint.Publish(
            new CloseChat(new ChatHubContextItemsHelper(Context.Items).UserId, chatId));
    }

    [Authorize(SupportUserRequirement.Policy)]
    public async Task AbandonChat(Guid chatId)
    {
        await publishEndpoint.Publish(
            new AbandonChat(new ChatHubContextItemsHelper(Context.Items).UserId, chatId));
    }
}

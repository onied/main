using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Support.Abstractions;
using Support.Authorization.Requirements;
using Support.Data;
using Support.Data.Models;
using Support.Dtos;
using Support.Helpers;

namespace Support.Hubs;

public class ChatHub(IChatManagementService chatManagementService, ILogger<ChatHub> logger, AppDbContext dbContext)
    : Hub<IChatClient>
{
    private const string SupportUserGroup = "SupportUsers";

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

        var isSupportUser = await dbContext.SupportUsers.FindAsync(userId) != null;
        if (isSupportUser)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, SupportUserGroup);
            items.IsSupportUser = true;
        }
    }

    public async Task SendMessage(string messageContent)
    {
        await chatManagementService.SendMessage(new ChatHubContextItemsHelper(Context.Items).UserId, messageContent);
    }

    public async Task MarkMessageAsRead(Guid messageId)
    {
        await chatManagementService.MarkMessageAsRead(new ChatHubContextItemsHelper(Context.Items).UserId, messageId);
    }

    [Authorize(SupportUserRequirement.Policy)]
    public async Task SendMessageToChat(Guid chatId, string messageContent)
    {
        await chatManagementService.SendMessageToChat(new ChatHubContextItemsHelper(Context.Items).UserId, chatId,
            messageContent);
    }

    [Authorize(SupportUserRequirement.Policy)]
    public async Task CloseChat(Guid chatId)
    {
        await chatManagementService.CloseChat(new ChatHubContextItemsHelper(Context.Items).UserId, chatId);
    }

    [Authorize(SupportUserRequirement.Policy)]
    public async Task AbandonChat(Guid chatId)
    {
        await chatManagementService.AbandonChat(new ChatHubContextItemsHelper(Context.Items).UserId, chatId);
    }
}

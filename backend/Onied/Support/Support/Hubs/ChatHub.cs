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

public class ChatHub(AppDbContext dbContext, ILogger<ChatHub> logger, IMapperBase mapper) : Hub<IChatClient>
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
        var items = new ChatHubContextItemsHelper(Context.Items);
        var chat = await dbContext.Chats.Where(chat => chat.ClientId == items.UserId).Include(chat => chat.Support)
            .SingleOrDefaultAsync();
        if (chat == null)
        {
            var chatId = Guid.NewGuid();
            chat = new Chat
            {
                Id = chatId,
                ClientId = items.UserId
            };
            dbContext.Chats.Add(chat);
        }

        if (chat.CurrentSessionId == null)
        {
            chat.CurrentSessionId = Guid.NewGuid();
            var systemMessage = new Message
            {
                Id = Guid.NewGuid(),
                Chat = chat,
                ChatId = chat.Id,
                UserId = items.UserId,
                CreatedAt = DateTime.UtcNow,
                ReadAt = null,
                MessageContent = $"open-session {chat.CurrentSessionId}",
                // TODO: Add system message generator service
                IsSystem = true
            };
            await SaveMessageAndSendToSupportUsers(systemMessage);
        }

        var message = new Message
        {
            Id = Guid.NewGuid(),
            Chat = chat,
            ChatId = chat.Id,
            UserId = items.UserId,
            CreatedAt = DateTime.UtcNow,
            ReadAt = null,
            MessageContent = messageContent,
            IsSystem = false
        };

        await SaveMessageAndSendToSupportUsers(message);

        await dbContext.SaveChangesAsync();
    }

    private async Task SaveMessageAndSendToSupportUsers(Message message)
    {
        dbContext.Messages.Add(message);

        var messageDto = mapper.Map<HubMessageDto>(message);
        if (message.Chat.SupportId == null)
            await Clients.Group(SupportUserGroup).ReceiveMessageFromChat(message.ChatId, messageDto);
        else
            await Clients.User(message.Chat.SupportId.Value.ToString())
                .ReceiveMessageFromChat(message.ChatId, messageDto);
    }

    public async Task MarkMessageAsRead(Guid messageId)
    {
        var items = new ChatHubContextItemsHelper(Context.Items);
        var message = await dbContext.Messages.FindAsync(messageId);
        if (message == null)
        {
            logger.LogWarning("Could not find message id: {id}", messageId);
            return;
        }

        if (message.UserId == items.UserId)
        {
            logger.LogWarning("User tried to mark their own message as read. UserId: {userId}, MessageId: {messageId}",
                items.UserId, messageId);
            return;
        }

        message.ReadAt = DateTime.UtcNow;
        dbContext.Update(message);
        await Clients.User(message.UserId.ToString()).ReceiveReadAt(message.Id, message.ReadAt.Value);

        await dbContext.SaveChangesAsync();
    }

    [Authorize(SupportUserRequirement.Policy)]
    public async Task SendMessageToChat(Guid chatId, string messageContent)
    {
        var items = new ChatHubContextItemsHelper(Context.Items);
        var chat = await dbContext.Chats.Where(chat => chat.Id == chatId).Include(chat => chat.Support)
            .SingleOrDefaultAsync();

        if (chat == null)
        {
            logger.LogWarning("Could not find chat with id: {id}", chatId);
            return;
        }

        if (chat.CurrentSessionId == null)
        {
            logger.LogWarning(
                "Support user tried to send message to the chat without an active session. UserId: {userId}, ChatId: {chatId}",
                items.UserId, chat.Id);
            return;
        }

        var message = new Message
        {
            Id = Guid.NewGuid(),
            Chat = chat,
            ChatId = chat.Id,
            UserId = items.UserId,
            CreatedAt = DateTime.UtcNow,
            ReadAt = null,
            MessageContent = messageContent,
            IsSystem = false
        };
        dbContext.Messages.Add(message);

        if (chat.SupportId == null)
            await Clients.Group(SupportUserGroup).RemoveChatFromOpened(chat.Id);

        chat.SupportId = items.UserId;
        dbContext.Chats.Update(chat);

        var messageDto = mapper.Map<HubMessageDto>(message);
        await Clients.User(chat.ClientId.ToString()).ReceiveMessage(messageDto);

        await dbContext.SaveChangesAsync();
    }

    [Authorize(SupportUserRequirement.Policy)]
    public async Task CloseChat(Guid chatId)
    {
        var items = new ChatHubContextItemsHelper(Context.Items);
        var chat = await dbContext.Chats.Where(chat => chat.Id == chatId).Include(chat => chat.Support)
            .SingleOrDefaultAsync();
        if (chat == null)
        {
            logger.LogWarning("Could not find chat with id: {id}", chatId);
            return;
        }

        if (chat.CurrentSessionId == null)
        {
            logger.LogWarning(
                "Support user tried to close session in a chat without an active session. UserId: {userId}, ChatId: {chatId}",
                items.UserId, chat.Id);
            return;
        }

        var message = new Message
        {
            Id = Guid.NewGuid(),
            Chat = chat,
            ChatId = chat.Id,
            UserId = items.UserId,
            CreatedAt = DateTime.UtcNow,
            ReadAt = null,
            MessageContent = "close-chat",
            // TODO: system message generator service
            IsSystem = true
        };
        dbContext.Messages.Add(message);

        if (chat.CurrentSessionId != null)
            await Clients.Group(SupportUserGroup).RemoveChatFromOpened(chat.Id);

        chat.CurrentSessionId = null;
        chat.SupportId = null;
        dbContext.Chats.Update(chat);

        var messageDto = mapper.Map<HubMessageDto>(message);
        await Clients.User(chat.ClientId.ToString()).ReceiveMessage(messageDto);

        await dbContext.SaveChangesAsync();
    }

    [Authorize(SupportUserRequirement.Policy)]
    public async Task AbandonChat(Guid chatId)
    {
        var items = new ChatHubContextItemsHelper(Context.Items);
        var chat = await dbContext.Chats.Where(chat => chat.Id == chatId).Include(chat => chat.Support)
            .SingleOrDefaultAsync();
        if (chat == null)
        {
            logger.LogWarning("Could not find chat with id: {id}", chatId);
            return;
        }

        if (chat.SupportId != items.UserId)
        {
            logger.LogWarning(
                "Support user tried to abandon chat they aren't appointed to. UserId: {userId}, ChatId: {chatId}",
                items.UserId, chatId);
            return;
        }

        if (chat.CurrentSessionId == null)
        {
            logger.LogError(
                "Support user tried to close a nonexistent session." +
                " This situation should not be reachable under normal operation. UserId: {userId}, ChatId: {chatId}",
                items.UserId, chatId);
            return;
        }

        var lastMessage = await dbContext.Messages.Where(message => message.ChatId == chatId)
            .OrderByDescending(message => message.CreatedAt).FirstOrDefaultAsync();

        if (lastMessage == null)
        {
            logger.LogError("Could not find last message, although an active session exists. ChatId: {chatId}", chatId);
            return;
        }

        chat.SupportId = null;
        chat.Support = null;
        dbContext.Chats.Update(chat);

        lastMessage.Chat = chat;

        var messageDto = mapper.Map<HubMessageDto>(lastMessage);
        await Clients.Group(SupportUserGroup).ReceiveMessageFromChat(chatId, messageDto);

        await dbContext.SaveChangesAsync();
    }
}

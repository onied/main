using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Support.Abstractions;
using Support.Data;
using Support.Data.Models;
using Support.Dtos;
using Support.Hubs;

namespace Support.Services;

public class ChatManagementService(
    ISystemMessageGenerator systemMessageGenerator,
    IHubContext<ChatHub, IChatClient> hubContext,
    AppDbContext dbContext,
    ILogger<ChatManagementService> logger,
    IMapperBase mapper) : IChatManagementService
{
    private const string SupportUserGroup = "SupportUsers";

    public async Task SendMessage(Guid senderId, string messageContent)
    {
        var chat = await dbContext.Chats.Where(chat => chat.ClientId == senderId).Include(chat => chat.Support)
            .SingleOrDefaultAsync();
        if (chat == null)
        {
            var chatId = Guid.NewGuid();
            chat = new Chat
            {
                Id = chatId,
                ClientId = senderId
            };
            dbContext.Chats.Add(chat);
        }

        if (chat.CurrentSessionId == null)
        {
            chat.CurrentSessionId = Guid.NewGuid();
            var systemMessage = systemMessageGenerator.GenerateOpenSessionMessage(chat, chat.CurrentSessionId.Value);
            await SaveMessageAndSendToSupportUsers(systemMessage);
        }

        var message = new Message
        {
            Id = Guid.NewGuid(),
            Chat = chat,
            ChatId = chat.Id,
            UserId = senderId,
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
            await hubContext.Clients.Group(SupportUserGroup).ReceiveMessageFromChat(message.ChatId, messageDto);
        else
            await hubContext.Clients.User(message.Chat.SupportId.Value.ToString())
                .ReceiveMessageFromChat(message.ChatId, messageDto);
    }

    public async Task MarkMessageAsRead(Guid senderId, Guid messageId)
    {
        var message = await dbContext.Messages.FindAsync(messageId);
        if (message == null)
        {
            logger.LogWarning("Could not find message id: {id}", messageId);
            return;
        }

        if (message.UserId == senderId)
        {
            logger.LogWarning("User tried to mark their own message as read. UserId: {userId}, MessageId: {messageId}",
                senderId, messageId);
            return;
        }

        message.ReadAt = DateTime.UtcNow;
        dbContext.Update(message);
        await hubContext.Clients.User(message.UserId.ToString()).ReceiveReadAt(message.Id, message.ReadAt.Value);

        await dbContext.SaveChangesAsync();
    }

    public async Task SendMessageToChat(Guid senderId, Guid chatId, string messageContent)
    {
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
                senderId, chat.Id);
            return;
        }

        var message = new Message
        {
            Id = Guid.NewGuid(),
            Chat = chat,
            ChatId = chat.Id,
            UserId = senderId,
            CreatedAt = DateTime.UtcNow,
            ReadAt = null,
            MessageContent = messageContent,
            IsSystem = false
        };
        dbContext.Messages.Add(message);

        if (chat.SupportId == null)
            await hubContext.Clients.Group(SupportUserGroup).RemoveChatFromOpened(chat.Id);

        chat.SupportId = senderId;
        dbContext.Chats.Update(chat);

        var messageDto = mapper.Map<HubMessageDto>(message);
        await hubContext.Clients.User(chat.ClientId.ToString()).ReceiveMessage(messageDto);

        await dbContext.SaveChangesAsync();
    }

    public async Task CloseChat(Guid senderId, Guid chatId)
    {
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
                senderId, chat.Id);
            return;
        }

        var message = systemMessageGenerator.GenerateCloseSessionMessage(chat, senderId);
        dbContext.Messages.Add(message);

        if (chat.CurrentSessionId != null)
            await hubContext.Clients.Group(SupportUserGroup).RemoveChatFromOpened(chat.Id);

        chat.CurrentSessionId = null;
        chat.SupportId = null;
        dbContext.Chats.Update(chat);

        var messageDto = mapper.Map<HubMessageDto>(message);
        await hubContext.Clients.User(chat.ClientId.ToString()).ReceiveMessage(messageDto);

        await dbContext.SaveChangesAsync();
    }

    public async Task AbandonChat(Guid senderId, Guid chatId)
    {
        var chat = await dbContext.Chats.Where(chat => chat.Id == chatId).Include(chat => chat.Support)
            .SingleOrDefaultAsync();
        if (chat == null)
        {
            logger.LogWarning("Could not find chat with id: {id}", chatId);
            return;
        }

        if (chat.SupportId != senderId)
        {
            logger.LogWarning(
                "Support user tried to abandon chat they aren't appointed to. UserId: {userId}, ChatId: {chatId}",
                senderId, chatId);
            return;
        }

        if (chat.CurrentSessionId == null)
        {
            logger.LogError(
                "Support user tried to close a nonexistent session." +
                " This situation should not be reachable under normal operation. UserId: {userId}, ChatId: {chatId}",
                senderId, chatId);
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
        await hubContext.Clients.Group(SupportUserGroup).ReceiveMessageFromChat(chatId, messageDto);

        await dbContext.SaveChangesAsync();
    }
}

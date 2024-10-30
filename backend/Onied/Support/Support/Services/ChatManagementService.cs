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
    IChatHubClientSender chatHubClientSender,
    IMessageGenerator messageGenerator,
    AppDbContext dbContext,
    ILogger<ChatManagementService> logger) : IChatManagementService
{

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
            var systemMessage = messageGenerator.GenerateOpenSessionMessage(chat, chat.CurrentSessionId.Value);
            dbContext.Messages.Add(systemMessage);
            await chatHubClientSender.SendMessageToSupportUsers(systemMessage);
        }

        var message = messageGenerator.GenerateMessage(senderId, chat, messageContent);

        dbContext.Messages.Add(message);
        await chatHubClientSender.SendMessageToSupportUsers(message);
        await chatHubClientSender.SendMessageToClient(message);

        await dbContext.SaveChangesAsync();
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
        await chatHubClientSender.NotifyMessageAuthorItWasRead(message);

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

        var message = messageGenerator.GenerateMessage(senderId, chat, messageContent);
        dbContext.Messages.Add(message);

        if (chat.SupportId == null)
            await chatHubClientSender.NotifySupportUsersOfTakenChat(chat);

        chat.SupportId = senderId;
        dbContext.Chats.Update(chat);

        await chatHubClientSender.SendMessageToClient(message);
        await chatHubClientSender.NotifySupportUserMessageAuthorItWasSent(message);

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

        var message = messageGenerator.GenerateCloseSessionMessage(chat, senderId);
        dbContext.Messages.Add(message);

        if (chat.CurrentSessionId != null)
            await chatHubClientSender.NotifySupportUsersOfTakenChat(chat);

        chat.CurrentSessionId = null;
        chat.SupportId = null;
        dbContext.Chats.Update(chat);

        await chatHubClientSender.SendMessageToClient(message);

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

        await chatHubClientSender.SendMessageToSupportUsers(lastMessage);

        await dbContext.SaveChangesAsync();
    }
}

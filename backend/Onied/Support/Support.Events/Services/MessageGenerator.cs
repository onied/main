using Support.Events.Abstractions;
using Support.Data.Models;

namespace Support.Events.Services;

public class MessageGenerator : IMessageGenerator
{
    public Message GenerateOpenSessionMessage(Chat chat, Guid sessionId)
    {
        return new Message
        {
            Id = Guid.NewGuid(),
            Chat = chat,
            ChatId = chat.Id,
            UserId = chat.ClientId,
            CreatedAt = DateTime.UtcNow,
            ReadAt = null,
            MessageContent = $"open-session {sessionId}",
            IsSystem = true
        };
    }

    public Message GenerateCloseSessionMessage(Chat chat, Guid supportId)
    {
        return new Message
        {
            Id = Guid.NewGuid(),
            Chat = chat,
            ChatId = chat.Id,
            UserId = supportId,
            CreatedAt = DateTime.UtcNow,
            ReadAt = null,
            MessageContent = "close-session",
            IsSystem = true
        };
    }

    public Message GenerateMessage(Guid senderId, Chat chat, string messageContent)
    {
        return new Message
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
    }
}

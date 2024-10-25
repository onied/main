using Support.Abstractions;
using Support.Data.Models;

namespace Support.Services;

public class SystemMessageGenerator : ISystemMessageGenerator
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
}

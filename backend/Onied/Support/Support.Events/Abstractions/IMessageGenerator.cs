using Support.Data.Models;

namespace Support.Events.Abstractions;

public interface IMessageGenerator
{
    Message GenerateOpenSessionMessage(Chat chat, Guid sessionId);
    Message GenerateCloseSessionMessage(Chat chat, Guid supportId);
    Message GenerateMessage(Guid senderId, Chat chat, string messageContent);
}

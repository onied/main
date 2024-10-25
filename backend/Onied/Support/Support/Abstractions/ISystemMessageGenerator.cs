using Support.Data.Models;

namespace Support.Abstractions;

public interface ISystemMessageGenerator
{
    Message GenerateOpenSessionMessage(Chat chat, Guid sessionId);
    Message GenerateCloseSessionMessage(Chat chat, Guid supportId);
}

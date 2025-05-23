using Support.Dtos;
using Support.Dtos.Hub.Response;

namespace Support.Abstractions;

public interface IChatClient
{
    Task ReceiveMessage(HubMessageDto message);
    Task ReceiveReadAt(Guid messageId, DateTime readAt);
    Task ReceiveMessageFromChat(Guid chatId, HubMessageDto message);
    Task RemoveChatFromOpened(Guid chatId);
}

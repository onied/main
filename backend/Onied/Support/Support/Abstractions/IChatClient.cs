using Support.Dtos;

namespace Support.Abstractions;

public interface IChatClient
{
    Task ReceiveMessage(HubMessageDto message);
    Task ReceiveReadAt(DateTime readAt);
    Task ReceiveMessageFromChat(Guid chatId, HubMessageDto messageDto);
    Task ReceiveReadAtInChat(Guid chatId, Guid messageId, DateTime readAt);
    Task RemoveChatFromOpened(Guid chatId);
}

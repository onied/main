namespace Support.Abstractions;

public interface IChatManagementService
{
    Task SendMessage(Guid senderId, string messageContent);
    Task MarkMessageAsRead(Guid senderId, Guid messageId);
    Task SendMessageToChat(Guid senderId, Guid chatId, string messageContent);
    Task CloseChat(Guid senderId, Guid chatId);
    Task AbandonChat(Guid senderId, Guid chatId);
}

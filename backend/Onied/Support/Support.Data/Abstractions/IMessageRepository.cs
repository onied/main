using Support.Data.Models;

namespace Support.Data.Abstractions;

public interface IMessageRepository
{
    public Task<Message?> GetAsync(Guid messageId);
    public Task<MessageView?> GetViewWithChatAsync(Guid messageId);
    public Task AddAsync(Message message);
    public Task UpdateAsync(Message message);
    public Task<Message?> GetLastMessageInChatAsync(Guid chatId);
}

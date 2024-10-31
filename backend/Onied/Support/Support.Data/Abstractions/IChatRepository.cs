using Support.Data.Models;

namespace Support.Data.Abstractions;

public interface IChatRepository
{
    public Task<Chat?> GetWithSupportAndMessagesByUserIdAsync(Guid userId);

    public Task<Chat?> GetWithSupportAndMessagesAsync(Guid id);

    public Task<List<Chat>> GetActiveChatsAsync(Guid userId);

    public Task<List<Chat>> GetOpenChatsAsync();

    public Task<Chat?> GetWithSupportByUserIdAsync(Guid userId);

    public Task<Chat?> GetWithSupportAsync(Guid chatId);

    public Task<Chat> CreateForUserAsync(Guid userId);

    public Task UpdateAsync(Chat chat);
}

using Support.Data.Models;

namespace Support.Data.Abstractions;

public interface IChatRepository
{
    public Task<Chat?> GetWithSupportAndMessagesByUserIdAsync(Guid userId);

    public Task<Chat?> GetWithSupportAndMessagesAsync(Guid id);

    public Task<List<Chat>> GetActiveChatsAsync(Guid userId);

    public Task<List<Chat>> GetOpenChatsAsync(Guid userId);
}

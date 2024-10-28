using Microsoft.EntityFrameworkCore;
using Support.Data.Abstractions;
using Support.Data.Models;

namespace Support.Data.Repositories;

public class ChatRepository(AppDbContext dbContext) : IChatRepository
{
    public async Task<Chat?> GetAsync(Guid id)
        => await dbContext.Chats.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);

    public async Task<Chat?> GetWithSupportAndMessagesAsync(Guid id)
        => await dbContext.Chats
            .AsNoTracking()
            .Include(c => c.Support)
            .Include(c => c.Messages)
                .ThenInclude(m => m.SupportUser)
            .FirstOrDefaultAsync(c => c.Id == id);


    public async Task<Chat?> GetWithSupportAndMessagesByUserIdAsync(Guid userId)
        => await dbContext.Chats
            .AsNoTracking()
            .Include(c => c.Support)
            .Include(c => c.Messages)
                .ThenInclude(m => m.SupportUser)
            .FirstOrDefaultAsync(c => c.ClientId == userId);

    public async Task<List<Chat>> GetActiveChatsAsync(Guid userId)
        => await dbContext.Chats
            .AsNoTracking()
            .Include(c => c.Support)
            .Include(c => c.Messages)
                .ThenInclude(m => m.SupportUser)
            .Where(c => c.Support != null && c.Support.Id == userId)
            .ToListAsync();

    public async Task<List<Chat>> GetOpenChatsAsync(Guid userId)
        => await dbContext.Chats
            .AsNoTracking()
            .Include(c => c.Support)
            .Include(c => c.Messages)
                .ThenInclude(m => m.SupportUser)
            .Where(c => c.CurrentSessionId != default && c.Support == null)
            .ToListAsync();
}

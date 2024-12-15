using Microsoft.EntityFrameworkCore;
using Support.Data.Abstractions;
using Support.Data.Models;

namespace Support.Data.Repositories;

public class ChatRepository(AppDbContext dbContext) : IChatRepository
{
    public async Task<Chat?> GetWithSupportAndMessagesAsync(Guid id)
    {
        return await dbContext.Chats
            .AsNoTracking()
            .Include(c => c.Support)
            .Include(c => c.Messages.OrderBy(message => message.CreatedAt))
            .ThenInclude(view => view.Files)
            .FirstOrDefaultAsync(c => c.Id == id);
    }


    public async Task<Chat?> GetWithSupportAndMessagesByUserIdAsync(Guid userId)
    {
        return await dbContext.Chats
            .AsNoTracking()
            .Include(c => c.Support)
            .Include(c => c.Messages.OrderBy(message => message.CreatedAt))
            .ThenInclude(view => view.Files)
            .FirstOrDefaultAsync(c => c.ClientId == userId);
    }

    public async Task<List<Chat>> GetActiveChatsAsync(Guid userId)
    {
        return await dbContext.Chats
            .AsNoTracking()
            .Include(c => c.Support)
            .Include(c => c.Messages.OrderBy(message => message.CreatedAt))
            .Where(c => c.Support != null && c.Support.Id == userId)
            .ToListAsync();
    }

    public async Task<List<Chat>> GetOpenChatsAsync()
    {
        return await dbContext.Chats
            .AsNoTracking()
            .Include(c => c.Support)
            .Include(c => c.Messages.OrderBy(message => message.CreatedAt))
            .Where(c => c.CurrentSessionId != default && c.Support == null)
            .ToListAsync();
    }

    public Task<Chat?> GetWithSupportByUserIdAsync(Guid userId)
    {
        return dbContext.Chats
            .Include(chat => chat.Support)
            .Where(chat => chat.ClientId == userId)
            .SingleOrDefaultAsync();
    }

    public Task<Chat?> GetWithSupportAsync(Guid chatId)
    {
        return dbContext.Chats
            .Include(chat => chat.Support)
            .Where(chat => chat.Id == chatId)
            .SingleOrDefaultAsync();
    }

    public async Task UpdateAsync(Chat chat)
    {
        dbContext.Chats.Update(chat);
        await dbContext.SaveChangesAsync();
    }
}

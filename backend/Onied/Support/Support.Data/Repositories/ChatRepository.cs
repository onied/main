using Microsoft.EntityFrameworkCore;
using Support.Data.Abstractions;
using Support.Data.Models;

namespace Support.Data.Repositories;

public class ChatRepository(AppDbContext dbContext) : IChatRepository
{
    public async Task<Chat?> GetAsync(Guid id)
        => await dbContext.Chats.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
}

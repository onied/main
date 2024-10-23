using Microsoft.EntityFrameworkCore;
using Support.Data.Abstractions;
using Support.Data.Models;

namespace Support.Data.Repositories;

public class MessageRepository(AppDbContext dbContext) : IMessageRepository
{
    public async Task<Message?> GetAsync(Guid id)
        => await dbContext.Messages.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
}

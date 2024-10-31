using Microsoft.EntityFrameworkCore;
using Support.Data.Abstractions;
using Support.Data.Models;

namespace Support.Data.Repositories;

public class SupportUserRepository(AppDbContext dbContext) : ISupportUserRepository
{
    public async Task<SupportUser?> GetAsync(Guid id)
        => await dbContext.SupportUsers.FirstOrDefaultAsync(c => c.Id == id);
}

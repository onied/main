using Purchases.Data.Abstractions;
using Purchases.Data.Models;

namespace Purchases.Data.Repositories;

public class UserRepository(AppDbContext dbContext) : IUserRepository
{
    public async Task AddAsync(User user)
    {
        await dbContext.Users.AddAsync(user);
        await dbContext.SaveChangesAsync();
    }

    public async Task RemoveAsync(User user)
    {
        dbContext.Users.Remove(user);
        await dbContext.SaveChangesAsync();
    }
}

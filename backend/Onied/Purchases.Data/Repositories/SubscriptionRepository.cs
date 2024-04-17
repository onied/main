using Microsoft.EntityFrameworkCore;
using Purchases.Data.Abstractions;
using Purchases.Data.Models;

namespace Purchases.Data.Repositories;

public class SubscriptionRepository(AppDbContext dbContext) : ISubscriptionRepository
{
    public async Task<Subscription?> GetAsync(int id, bool withUsers = false)
    {
        var queue = dbContext.Subscriptions.AsNoTracking().AsQueryable();
        if (withUsers) queue = queue.Include(s => s.Users);

        return await queue.FirstOrDefaultAsync(c => c.Id == id);
    }
    public async Task AddAsync(Subscription subscription)
    {
        await dbContext.Subscriptions.AddAsync(subscription);
        await dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Subscription subscription)
    {
        dbContext.Subscriptions.Update(subscription);
        await dbContext.SaveChangesAsync();
    }

    public async Task RemoveAsync(Subscription subscription)
    {
        dbContext.Subscriptions.Remove(subscription);
        await dbContext.SaveChangesAsync();
    }
}

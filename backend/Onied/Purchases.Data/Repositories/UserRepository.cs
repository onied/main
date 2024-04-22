using Microsoft.EntityFrameworkCore;
using Purchases.Data.Abstractions;
using Purchases.Data.Models;
using Purchases.Data.Models.PurchaseDetails;

namespace Purchases.Data.Repositories;

public class UserRepository(AppDbContext dbContext) : IUserRepository
{
    public async Task<User?> GetAsync(Guid id, bool withPurchases = false, bool withSubscription = false)
    {
        var query = dbContext.Users.AsNoTracking().AsQueryable();
        if (withPurchases) query = query
            .Include(u => u.Purchases)
            .ThenInclude(p => p.PurchaseDetails)
            .ThenInclude(pd => ((CoursePurchaseDetails)pd).Course)
            .Include(u => u.Purchases)
            .ThenInclude(p => p.PurchaseDetails)
            .ThenInclude(pd => ((CertificatePurchaseDetails)pd).Course)
            .Include(u => u.Purchases)
            .ThenInclude(p => p.PurchaseDetails)
            .ThenInclude(pd => ((SubscriptionPurchaseDetails)pd).Subscription);

        if (withSubscription) query = query
            .Include(u => u.Subscription);

        return await query.FirstOrDefaultAsync(u => u.Id == id);
    }

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

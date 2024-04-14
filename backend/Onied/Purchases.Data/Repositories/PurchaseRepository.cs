using Microsoft.EntityFrameworkCore;
using Purchases.Data.Abstractions;
using Purchases.Data.Models;
using Purchases.Data.Models.PurchaseDetails;

namespace Purchases.Data.Repositories;

public class PurchaseRepository(AppDbContext dbContext) : IPurchaseRepository
{
    public async Task<Purchase?> GetAsync(int id)
        => await dbContext.Purchases
            .AsNoTracking()
            .Include(p => p.PurchaseDetails)
            .Include(p => p.User)
            .FirstOrDefaultAsync(p => p.Id == id);

    public async Task AddAsync(Purchase purchase, PurchaseDetails purchaseDetails)
    {
        await dbContext.Purchases.AddAsync(purchase);
        await dbContext.PurchaseDetails.AddAsync(purchaseDetails);
        await dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Purchase purchase)
    {
        dbContext.Purchases.Update(purchase);
        await dbContext.SaveChangesAsync();
    }

    public async Task RemoveAsync(Purchase purchase)
    {
        dbContext.Purchases.Remove(purchase);
        await dbContext.SaveChangesAsync();
    }
}

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

    public async Task<Purchase> AddAsync(Purchase purchase, PurchaseDetails purchaseDetails)
    {
        await using var transaction = await dbContext.Database.BeginTransactionAsync();

        var purchaseSaved = await dbContext.Purchases.AddAsync(purchase);
        // todo избавиться от двух записей в бд, сделав foreign key.
        await dbContext.SaveChangesAsync();
        purchaseDetails.Id = purchaseSaved.Entity.Id;
        await dbContext.PurchaseDetails.AddAsync(purchaseDetails);
        await dbContext.SaveChangesAsync();

        await transaction.CommitAsync();
        return purchaseSaved.Entity;
    }

    public async Task<Purchase> UpdateAsync(Purchase purchase)
    {
        var purchaseSaved = dbContext.Purchases.Update(purchase);
        await dbContext.SaveChangesAsync();
        return purchaseSaved.Entity;
    }

    public async Task RemoveAsync(Purchase purchase)
    {
        dbContext.Purchases.Remove(purchase);
        await dbContext.SaveChangesAsync();
    }

    public async Task<bool> UpdateAutoRenewal(int subscriptionId)
    {
        var purchaseSubscription = await dbContext.Purchases
            .Where(p => p.Id == subscriptionId)
            .Include(purchase => purchase.PurchaseDetails)
            .FirstOrDefaultAsync(p => p.Id == subscriptionId);

        if (purchaseSubscription != null
            && purchaseSubscription.PurchaseDetails.GetType() == typeof(SubscriptionPurchaseDetails))
        {
            var subscriptionDetails = (SubscriptionPurchaseDetails)purchaseSubscription.PurchaseDetails;
            subscriptionDetails.AutoRenewalEnabled = !subscriptionDetails.AutoRenewalEnabled;
            await dbContext.SaveChangesAsync();

            return true;
        }

        return false;
    }
}

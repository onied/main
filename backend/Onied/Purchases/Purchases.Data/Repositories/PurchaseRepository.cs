using Microsoft.EntityFrameworkCore;
using Purchases.Data.Abstractions;
using Purchases.Data.Enums;
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
        purchase.PurchaseDetails = purchaseDetails;
        var purchaseSaved = await dbContext.Purchases.AddAsync(purchase);
        await dbContext.SaveChangesAsync();
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

    public async Task<bool> UpdateAutoRenewal(Guid userId, int subscriptionId, bool autoRenewal)
    {
        var purchaseSubscription = await dbContext.Purchases
            .Where(p => p.UserId == userId && p.PurchaseDetails.PurchaseType == PurchaseType.Subscription &&
                        (p.PurchaseDetails as SubscriptionPurchaseDetails)!.Subscription.Id == subscriptionId)
            .Include(purchase => purchase.PurchaseDetails)
            .FirstOrDefaultAsync();

        if (purchaseSubscription is not { PurchaseDetails: SubscriptionPurchaseDetails subscriptionDetails }
            || purchaseSubscription.UserId != userId) return false;

        subscriptionDetails.AutoRenewalEnabled = autoRenewal;
        await dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> AddDaysToSubscriptionEndDate(Guid userId, int subscriptionId, TimeSpan addDays)
    {
        var purchaseSubscription = await dbContext.Purchases
            .Where(p => p.UserId == userId
                        && p.PurchaseDetails.PurchaseType == PurchaseType.Subscription
                        && (p.PurchaseDetails as SubscriptionPurchaseDetails)!.Subscription.Id == subscriptionId)
            .Include(purchase => purchase.PurchaseDetails)
            .FirstOrDefaultAsync();

        if (purchaseSubscription is not { PurchaseDetails: SubscriptionPurchaseDetails subscriptionDetails }
            || purchaseSubscription.UserId != userId) return false;

        subscriptionDetails.EndDate = subscriptionDetails.EndDate.Add(addDays);
        await dbContext.SaveChangesAsync();
        return true;
    }

    public async Task UpdateSubscriptionWithAutoRenewal()
    {
        var purchases = dbContext.Purchases
            .Include(purchase => purchase.PurchaseDetails);

        foreach (var purchase in purchases)
        {
            if (purchase.PurchaseDetails is SubscriptionPurchaseDetails subscriptionDetails &&
                subscriptionDetails.EndDate.Date <= DateTime.UtcNow.Date &&
                subscriptionDetails.AutoRenewalEnabled)
            {
                subscriptionDetails.EndDate = DateTime.UtcNow.Date + TimeSpan.FromDays(30);
            }
        }

        await dbContext.SaveChangesAsync();
    }
}

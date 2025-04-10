using Purchases.Data.Models;
using Purchases.Data.Models.PurchaseDetails;

namespace Purchases.Data.Abstractions;

public interface IPurchaseRepository
{
    public Task<Purchase?> GetAsync(int id);
    public Task<Purchase> AddAsync(Purchase purchase, PurchaseDetails purchaseDetails);
    public Task<Purchase> UpdateAsync(Purchase purchase);
    public Task RemoveAsyncById(int purchaseId);

    public Task<bool> UpdateAutoRenewal(Guid userId, int subscriptionId, bool autoRenewal);
    public Task<bool> AddDaysToSubscriptionEndDate(Guid userId, int subscriptionId, TimeSpan addDays);

    public Task UpdateSubscriptionWithAutoRenewal();
}

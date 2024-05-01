using Purchases.Data.Models;
using Purchases.Data.Models.PurchaseDetails;

namespace Purchases.Data.Abstractions;

public interface IPurchaseRepository
{
    public Task<Purchase?> GetAsync(int id);
    public Task<Purchase> AddAsync(Purchase purchase, PurchaseDetails purchaseDetails);
    public Task<Purchase> UpdateAsync(Purchase purchase);
    public Task RemoveAsync(Purchase purchase);

    public Task<bool> UpdateAutoRenewal(int subscriptionId);

    public Task UpdateSubscriptionWithAutoRenewal();
}

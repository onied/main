using Purchases.Data.Models;
using Purchases.Data.Models.PurchaseDetails;

namespace Purchases.Data.Abstractions;

public interface IPurchaseRepository
{
    public Task<Purchase?> GetAsync(int id);
    public Task AddAsync(Purchase purchase, PurchaseDetails purchaseDetails);
    public Task UpdateAsync(Purchase purchase);
    public Task RemoveAsync(Purchase purchase);
}

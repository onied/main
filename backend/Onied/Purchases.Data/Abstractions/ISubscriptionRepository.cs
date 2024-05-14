using Purchases.Data.Models;

namespace Purchases.Data.Abstractions;

public interface ISubscriptionRepository
{
    public Task<Subscription?> GetAsync(int id, bool withUsers = false);
    public Task<List<Subscription>> GetAllSubscriptions();
    public Task AddAsync(Subscription subscription);
    public Task UpdateAsync(Subscription subscription);
    public Task RemoveAsync(Subscription subscription);
}

using Purchases.Data.Models;

namespace Purchases.Data.Abstractions;

public interface IUserRepository
{
    public Task<User?> GetAsync(Guid id, bool withPurchases = false, bool withSubscription = false);
    public Task AddAsync(User user);
    public Task UpdateAsync(User user);
    public Task RemoveAsync(User user);
}

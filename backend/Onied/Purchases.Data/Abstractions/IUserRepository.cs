using Purchases.Data.Models;

namespace Purchases.Data.Abstractions;

public interface IUserRepository
{
    public Task<User?> GetAsync(Guid id, bool withPurchases = false);
    public Task AddAsync(User user);
    public Task RemoveAsync(User user);
}

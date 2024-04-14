using Purchases.Data.Models;

namespace Purchases.Data.Abstractions;

public interface IUserRepository
{
    public Task AddAsync(User user);
    public Task RemoveAsync(User user);
}

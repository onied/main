using Support.Data.Models;

namespace Support.Data.Abstractions;

public interface IChatRepository
{
    public Task<Chat?> GetAsync(Guid id);
}

using Support.Data.Models;

namespace Support.Data.Abstractions;

public interface ISupportUserRepository
{
    public Task<SupportUser?> GetAsync(Guid id);
}

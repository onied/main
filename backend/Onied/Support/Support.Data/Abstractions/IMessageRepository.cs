using Support.Data.Models;

namespace Support.Data.Abstractions;

public interface IMessageRepository
{
    public Task<Message?> GetAsync(Guid id);
}

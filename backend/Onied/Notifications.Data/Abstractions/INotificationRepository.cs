using Notifications.Data.Models;

namespace Notifications.Data.Abstractions;

public interface INotificationRepository
{
    public Task AddAsync(Notification notification);
    public Task<List<Notification>> GetRangeByUser(Guid userId);
}

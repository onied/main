using Notifications.Data.Models;

namespace Notifications.Data.Abstractions;

public interface INotificationRepository
{
    public Task<Notification> AddAsync(Notification notification);
    public Task<Notification?> GetAsync(int id);
    public Task<Notification> UpdateAsync(Notification notification);
    public Task<List<Notification>> GetRangeByUserAsync(Guid userId);
}

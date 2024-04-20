using Microsoft.EntityFrameworkCore;
using Notifications.Data.Abstractions;
using Notifications.Data.Models;

namespace Notifications.Data.Repositories;

public class NotificationRepository(AppDbContext dbContext) : INotificationRepository
{
    public async Task<Notification> AddAsync(Notification notification)
    {
        var storedNotificationEntry = await dbContext.Notifications.AddAsync(notification);
        await dbContext.SaveChangesAsync();
        return storedNotificationEntry.Entity;
    }

    public async Task<List<Notification>> GetRangeByUserAsync(Guid userId)
        => await dbContext.Notifications
            .AsNoTracking()
            .Where(n => n.UserId == userId).ToListAsync();
}

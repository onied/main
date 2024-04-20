using Microsoft.EntityFrameworkCore;
using Notifications.Data.Abstractions;
using Notifications.Data.Models;

namespace Notifications.Data.Repositories;

public class NotificationRepository(AppDbContext dbContext) : INotificationRepository
{
    public async Task AddAsync(Notification notification)
    {
        await dbContext.Notifications.AddAsync(notification);
        await dbContext.SaveChangesAsync();
    }

    public async Task<List<Notification>> GetRangeByUser(Guid userId)
        => await dbContext.Notifications
            .AsNoTracking()
            .Where(n => n.UserId == userId).ToListAsync();
}

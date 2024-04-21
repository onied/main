using Courses.Services.Abstractions;
using MassTransit.Data.Messages;

namespace Courses.Services;

public class NotificationPreparerService : INotificationPreparerService
{
    public NotificationSent PrepareNotification(NotificationSent notification)
        => notification with
        {
            Title = notification.Title.Length > 100
                ? string.Concat(notification.Title.AsSpan(0, 97), "...")
                : notification.Title,
            Message = notification.Message.Length > 350
                ? string.Concat(notification.Message.AsSpan(0, 347), "...")
                : notification.Message,
        };
}

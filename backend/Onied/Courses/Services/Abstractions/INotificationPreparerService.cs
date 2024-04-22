using MassTransit.Data.Messages;

namespace Courses.Services.Abstractions;

public interface INotificationPreparerService
{
    public NotificationSent PrepareNotification(NotificationSent notification);
}

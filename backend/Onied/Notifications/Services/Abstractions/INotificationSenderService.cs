using MassTransit.Data.Messages;

namespace Notifications.Services.Abstractions;

public interface INotificationSenderService
{
    public Task Send(NotificationSent notificationSent);
}

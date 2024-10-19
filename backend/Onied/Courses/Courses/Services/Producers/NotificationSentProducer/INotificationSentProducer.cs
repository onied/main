using MassTransit.Data.Messages;

namespace Courses.Services.Producers.NotificationSentProducer;

public interface INotificationSentProducer
{
    public Task PublishForAll(NotificationSent notificationSent);
    public Task PublishForOne(NotificationSent notificationSent);
}

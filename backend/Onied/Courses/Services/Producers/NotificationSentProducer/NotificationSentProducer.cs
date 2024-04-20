using Courses.Services.Abstractions;
using MassTransit;
using MassTransit.Data.Messages;

namespace Courses.Services.Producers.NotificationSentProducer;

public class NotificationSentProducer(
    ILogger<NotificationSentProducer> logger,
    IUserRepository userRepository,
    IPublishEndpoint publishEndpoint
    ) : INotificationSentProducer
{
    public async Task PublishForAll(NotificationSent notificationSent)
    {
        if (notificationSent.UserId != Guid.Empty)
        {
            logger.LogError("Error occured while sending notification, " +
                            "mass notification can only have UserId=Guid.Empty");
            throw new InvalidOperationException();
        }

        var allUsersNotifications = (await userRepository
            .GetUsersWithConditionAsync())
            .Select(u => notificationSent with { UserId = u.Id });

        foreach (var notification in allUsersNotifications)
            await publishEndpoint.Publish(notification);
    }

    public async Task PublishForOne(NotificationSent notificationSent)
    {
        if (notificationSent.UserId == Guid.Empty)
        {
            logger.LogError("Error occured while sending notification, " +
                            "notification for concrete user cannot have UserId=Guid.Empty");
            throw new InvalidOperationException();
        }
        await publishEndpoint.Publish(notificationSent);
    }
}

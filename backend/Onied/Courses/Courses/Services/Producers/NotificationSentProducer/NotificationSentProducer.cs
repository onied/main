using System.ComponentModel.DataAnnotations;
using Common.Validation;
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
    private readonly DataAnnotationValidator _validator = new();
    public async Task PublishForAll(NotificationSent notificationSent)
    {

        var allUsersNotifications =
            (await userRepository.GetUsersWithConditionAsync())
            .Select(u => ValidateAndPrepareNotification(notificationSent with { UserId = u.Id }));

        foreach (var notification in allUsersNotifications)
            await publishEndpoint.Publish(notification);
    }

    public async Task PublishForOne(NotificationSent notificationSent)
    {
        var notification = ValidateAndPrepareNotification(notificationSent);
        await publishEndpoint.Publish(notification);
    }

    private NotificationSent ValidateAndPrepareNotification(NotificationSent notification)
    {
        notification = NotificationSent.Normalize(notification);

        if (_validator.TryValidate(notification, out var results)) return notification;

        logger.LogError("Error occured while sending notification, NotificationSent is invalid");
        foreach (var r in results)
            logger.LogError("NotificationSent validation error: {errorMessage}", r.ErrorMessage);

        throw new ValidationException();
    }
}

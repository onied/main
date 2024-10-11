using System.ComponentModel.DataAnnotations;
using Common.Validation;
using Courses.Services.Abstractions;
using MassTransit;
using MassTransit.Data.Messages;

namespace Courses.Services.Producers.NotificationSentProducer;

public class NotificationSentProducer(
    ILogger<NotificationSentProducer> logger,
    IUserRepository userRepository,
    INotificationPreparerService notificationPreparerService,
    IPublishEndpoint publishEndpoint
) : INotificationSentProducer
{
    public async Task PublishForAll(NotificationSent notificationSent)
    {
        var allUsersNotifications = (await userRepository.GetUsersWithConditionAsync())
            .Select(
                u =>
                {
                    var notification = notificationPreparerService
                        .PrepareNotification(notificationSent with { UserId = u.Id });
        
                    var validator = new DataAnnotationValidator();
                    if (validator.TryValidate(notification, out var results)) return notification;
                    
                    logger.LogError("Error occured while sending notification, NotificationSent is invalid");
                    foreach (var r in results)
                        logger.LogError("NotificationSent validation error: {errorMessage}", r.ErrorMessage);
            
                    throw new ValidationException();
                }
            );

        foreach (var notification in allUsersNotifications)
            await publishEndpoint.Publish(notification);
    }

    public async Task PublishForOne(NotificationSent notificationSent)
    {
        var notification = notificationPreparerService
            .PrepareNotification(notificationSent);
        
        var validator = new DataAnnotationValidator();
        if (!validator.TryValidate(notification, out var results))
        {
            logger.LogError("Error occured while sending notification, NotificationSent is invalid");
            foreach (var r in results)
                logger.LogError("NotificationSent validation error: {errorMessage}", r.ErrorMessage);
            
            throw new ValidationException();
        }

        await publishEndpoint.Publish(notification);
    }
}

using MassTransit;
using MassTransit.Data.Messages;
using Notifications.Services.Abstractions;

namespace Notifications.Services.Consumers;

public class NotificationSentConsumer(
    INotificationSenderService notificationSenderService
) : IConsumer<NotificationSent>
{
    public async Task Consume(ConsumeContext<NotificationSent> context)
    {
        await notificationSenderService.Send(context.Message);
    }
}

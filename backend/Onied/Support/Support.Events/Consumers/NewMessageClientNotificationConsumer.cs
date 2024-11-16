using MassTransit;
using MassTransit.Data.Messages;
using Microsoft.Extensions.Logging;
using Support.Data.Abstractions;
using Support.Events.Messages;

namespace Support.Events.Consumers;

public class NewMessageClientNotificationConsumer(
    IMessageRepository messageRepository,
    IPublishEndpoint publishEndpoint,
    ILogger<NewMessageClientNotificationConsumer> logger)
    : IConsumer<NewMessageClientNotification>
{
    public async Task Consume(ConsumeContext<NewMessageClientNotification> context)
    {
        var message = await messageRepository.GetViewWithChatAsync(context.Message.MessageId);
        if (message == null)
        {
            logger.LogWarning("Could not find message in view.");
            return;
        }

        if (message is { SupportNumberNullIfUser: not null, ReadAt: null })
        {
            var notification = NotificationSent.Normalize(new NotificationSent(
                $"Сообщение от оператора #{message.SupportNumberNullIfUser}",
                message.MessageContent, message.Chat.ClientId,
                "https://upload.wikimedia.org/wikipedia/commons/7/7c/Profile_avatar_placeholder_large.png"));
            await publishEndpoint.Publish(notification);
        }
    }
}

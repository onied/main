using MassTransit;
using MassTransit.Data.Messages;
using MassTransit.DependencyInjection;
using Support.Abstractions;
using Support.Data.Abstractions;
using Support.Messages;

namespace Support.Services.Consumers;

public class NewMessageClientNotificationConsumer(
    IMessageRepository messageRepository,
    Bind<INotificationBus, IPublishEndpoint> publishEndpoint,
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
            await publishEndpoint.Value.Publish(notification);
        }
    }
}

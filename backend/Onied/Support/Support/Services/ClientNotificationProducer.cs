using MassTransit;
using MassTransit.Data.Messages;
using Support.Abstractions;
using Support.Data.Models;

namespace Support.Services;

public class ClientNotificationProducer(IPublishEndpoint publishEndpoint) : IClientNotificationProducer
{
    public async Task NotifyClientOfNewMessage(Message message)
    {
        var notification = NotificationSent.Normalize(new NotificationSent(
            "Сообщение" + (message.Chat.Support == null ? "" : $" от оператора #{message.Chat.Support.Number}"),
            message.MessageContent, message.Chat.ClientId));
        await publishEndpoint.Publish(notification);
    }
}

using MassTransit;
using Microsoft.Extensions.Logging;
using Support.Data.Abstractions;
using Support.Events.Abstractions;
using Support.Events.Messages;

namespace Support.Events.Consumers;

public class MarkMessageAsReadConsumer(
    IChatHubClientSender chatHubClientSender,
    IMessageRepository messageRepository,
    ILogger<MarkMessageAsReadConsumer> logger) : IConsumer<MarkMessageAsRead>
{
    public async Task Consume(ConsumeContext<MarkMessageAsRead> context)
    {
        var message = await messageRepository.GetAsync(context.Message.MessageId);
        if (message == null)
        {
            logger.LogWarning("Could not find message id: {id}", context.Message.MessageId);
            return;
        }

        if (message.UserId == context.Message.SenderId)
        {
            logger.LogWarning("User tried to mark their own message as read. UserId: {userId}, MessageId: {messageId}",
                context.Message.SenderId, context.Message.MessageId);
            return;
        }

        message.ReadAt = DateTime.UtcNow;
        await messageRepository.UpdateAsync(message);
        await chatHubClientSender.NotifyMessageAuthorItWasRead(message);
    }
}

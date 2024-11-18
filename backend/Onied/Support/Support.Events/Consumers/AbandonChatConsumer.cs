using MassTransit;
using Microsoft.Extensions.Logging;
using Support.Data.Abstractions;
using Support.Events.Abstractions;
using Support.Events.Messages;

namespace Support.Events.Consumers;

public class AbandonChatConsumer(
    IChatHubClientSender chatHubClientSender,
    IChatRepository chatRepository,
    IMessageRepository messageRepository,
    ILogger<AbandonChatConsumer> logger) : IConsumer<AbandonChat>
{
    public async Task Consume(ConsumeContext<AbandonChat> context)
    {
        var chat = await chatRepository.GetWithSupportAsync(context.Message.ChatId);
        if (chat == null)
        {
            logger.LogWarning("Could not find chat with id: {id}", context.Message.ChatId);
            return;
        }

        if (chat.SupportId != context.Message.SenderId)
        {
            logger.LogWarning(
                "Support user tried to abandon chat they aren't appointed to. UserId: {userId}, ChatId: {chatId}",
                context.Message.SenderId, context.Message.ChatId);
            return;
        }

        if (chat.CurrentSessionId == null)
        {
            logger.LogError(
                "Support user tried to close a nonexistent session." +
                " This situation should not be reachable under normal operation. UserId: {userId}, ChatId: {chatId}",
                context.Message.SenderId, context.Message.ChatId);
            return;
        }

        var lastMessage = await messageRepository.GetLastMessageInChatAsync(context.Message.ChatId);

        if (lastMessage == null)
        {
            logger.LogError("Could not find last message, although an active session exists. ChatId: {chatId}",
                context.Message.ChatId);
            return;
        }

        chat.SupportId = null;
        chat.Support = null;
        await chatRepository.UpdateAsync(chat);

        lastMessage.Chat = chat;
        await chatHubClientSender.SendMessageToSupportUsers(lastMessage);
    }
}

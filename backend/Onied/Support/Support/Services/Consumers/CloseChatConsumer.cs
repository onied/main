using MassTransit;
using Support.Abstractions;
using Support.Data.Abstractions;
using Support.Messages;

namespace Support.Services.Consumers;

public class CloseChatConsumer(
    IChatHubClientSender chatHubClientSender,
    IChatRepository chatRepository,
    IMessageGenerator messageGenerator,
    IMessageRepository messageRepository,
    ILogger<CloseChatConsumer> logger) : IConsumer<CloseChat>
{
    public async Task Consume(ConsumeContext<CloseChat> context)
    {
        var chat = await chatRepository.GetWithSupportAsync(context.Message.ChatId);
        if (chat == null)
        {
            logger.LogWarning("Could not find chat with id: {id}", context.Message.ChatId);
            return;
        }

        if (chat.CurrentSessionId == null)
        {
            logger.LogWarning(
                "Support user tried to close session in a chat without an active session. UserId: {userId}, ChatId: {chatId}",
                context.Message.SenderId, chat.Id);
            return;
        }

        var message = messageGenerator.GenerateCloseSessionMessage(chat, context.Message.SenderId);
        await messageRepository.AddAsync(message);

        if (chat.CurrentSessionId != null)
            await chatHubClientSender.NotifySupportUsersOfTakenChat(chat);

        chat.CurrentSessionId = null;
        chat.SupportId = null;
        await chatRepository.UpdateAsync(chat);

        chat.Support = null;
        message.Chat = chat;
        await chatHubClientSender.SendMessageToClient(message);
    }
}

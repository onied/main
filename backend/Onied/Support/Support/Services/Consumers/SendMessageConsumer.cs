using MassTransit;
using Support.Abstractions;
using Support.Data.Abstractions;
using Support.Data.Models;
using Support.Messages;

namespace Support.Services.Consumers;

public class SendMessageConsumer(
    IChatHubClientSender chatHubClientSender,
    IMessageGenerator messageGenerator,
    IChatRepository chatRepository,
    IMessageRepository messageRepository) : IConsumer<SendMessage>
{
    public async Task Consume(ConsumeContext<SendMessage> context)
    {
        var chat = await chatRepository.GetWithSupportByUserIdAsync(context.Message.SenderId)
                   ?? await chatRepository.CreateForUserAsync(context.Message.SenderId);

        if (chat.CurrentSessionId == null)
        {
            chat.CurrentSessionId = Guid.NewGuid();
            var systemMessage = messageGenerator.GenerateOpenSessionMessage(chat, chat.CurrentSessionId.Value);
            await messageRepository.AddAsync(systemMessage);
            await chatHubClientSender.SendMessageToSupportUsers(systemMessage);
            await chatHubClientSender.SendMessageToClient(systemMessage);
        }

        var message = messageGenerator.GenerateMessage(context.Message.SenderId, chat, context.Message.MessageContent);
        await messageRepository.AddAsync(message);

        await chatHubClientSender.SendMessageToSupportUsers(message);
        await chatHubClientSender.SendMessageToClient(message);
    }
}

using MassTransit;
using Support.Data.Abstractions;
using Support.Data.Models;
using Support.Events.Abstractions;
using Support.Events.Messages;
using File = Support.Data.Models.File;

namespace Support.Events.Consumers;

public class SendMessageConsumer(
    IChatHubClientSender chatHubClientSender,
    IMessageGenerator messageGenerator,
    IChatRepository chatRepository,
    IMessageRepository messageRepository) : IConsumer<SendMessage>
{
    public async Task Consume(ConsumeContext<SendMessage> context)
    {
        var chat = await chatRepository.GetWithSupportByUserIdAsync(context.Message.SenderId)
                   ?? new Chat
                   {
                       Id = Guid.NewGuid(),
                       ClientId = context.Message.SenderId
                   };

        if (chat.CurrentSessionId == null)
        {
            chat.CurrentSessionId = Guid.NewGuid();
            var systemMessage = messageGenerator.GenerateOpenSessionMessage(chat, chat.CurrentSessionId.Value);
            await messageRepository.AddAsync(systemMessage);
            await chatHubClientSender.SendMessageToSupportUsers(systemMessage);
            await chatHubClientSender.SendMessageToClient(systemMessage);
        }

        var message = messageGenerator.GenerateMessage(context.Message.SenderId, chat, context.Message.MessageContent);
        message.Files = context.Message.Files.Select(file => new File()
        {
            Id = Guid.NewGuid(),
            Filename = file.Filename,
            FileUrl = file.FileUrl
        }).ToList();
        await messageRepository.AddAsync(message);

        await chatHubClientSender.SendMessageToSupportUsers(message);
        await chatHubClientSender.SendMessageToClient(message);
    }
}

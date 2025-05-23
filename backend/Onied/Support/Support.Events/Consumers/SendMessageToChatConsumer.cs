using MassTransit;
using Microsoft.Extensions.Logging;
using Support.Data.Abstractions;
using Support.Events.Abstractions;
using Support.Events.Messages;
using File = Support.Data.Models.File;

namespace Support.Events.Consumers;

public class SendMessageToChatConsumer(
    IChatHubClientSender chatHubClientSender,
    IMessageGenerator messageGenerator,
    IChatRepository chatRepository,
    IMessageRepository messageRepository,
    ISupportUserRepository supportUserRepository,
    ILogger<SendMessageToChatConsumer> logger) : IConsumer<SendMessageToChat>
{
    public async Task Consume(ConsumeContext<SendMessageToChat> context)
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
                "Support user tried to send message to the chat without an active session. UserId: {userId}, ChatId: {chatId}",
                context.Message.SenderId, chat.Id);
            return;
        }

        var supportUser = await supportUserRepository.GetAsync(context.Message.SenderId);
        if (supportUser == null)
        {
            logger.LogError(
                "Sender was not a support user. UserId: {userId}", context.Message.SenderId);
            return;
        }

        if (chat.SupportId == null)
            await chatHubClientSender.NotifySupportUsersOfTakenChat(chat);

        chat.SupportId = supportUser.Id;
        chat.Support = supportUser;
        await chatRepository.UpdateAsync(chat);

        var message = messageGenerator.GenerateMessage(context.Message.SenderId, chat, context.Message.MessageContent);
        message.Files = context.Message.Files.Select(file => new File()
        {
            Id = Guid.NewGuid(),
            Filename = file.Filename,
            FileUrl = file.FileUrl
        }).ToList();
        await messageRepository.AddAsync(message);

        message.Chat = chat;
        await chatHubClientSender.SendMessageToClient(message);
        await chatHubClientSender.NotifySupportUserMessageAuthorItWasSent(message);
    }
}

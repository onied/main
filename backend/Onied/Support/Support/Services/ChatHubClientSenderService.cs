using AutoMapper;
using MassTransit;
using MassTransit.DependencyInjection;
using Microsoft.AspNetCore.SignalR;
using Support.Abstractions;
using Support.Data.Models;
using Support.Dtos;
using Support.Hubs;
using Support.Messages;

namespace Support.Services;

public class ChatHubClientSenderService(
    IHubContext<ChatHub, IChatClient> hubContext,
    Bind<IMassTransitInMemoryBus, IMessageScheduler> messageScheduler,
    IMapper mapper) : IChatHubClientSender
{
    public async Task SendMessageToSupportUsers(Message message)
    {
        var messageDto = mapper.Map<HubMessageDto>(message);
        if (message.Chat.SupportId == null)
            await hubContext.Clients.Group(ChatHub.SupportUserGroup).ReceiveMessageFromChat(message.ChatId, messageDto);
        else
            await hubContext.Clients.User(message.Chat.SupportId.Value.ToString())
                .ReceiveMessageFromChat(message.ChatId, messageDto);
    }

    public async Task SendMessageToClient(Message message)
    {
        var messageDto = mapper.Map<HubMessageDto>(message);
        await hubContext.Clients.User(message.Chat.ClientId.ToString()).ReceiveMessage(messageDto);
        await messageScheduler.Value.SchedulePublish(DateTime.UtcNow.AddMinutes(1),
            new NewMessageClientNotification(message.Id));
    }

    public async Task NotifyMessageAuthorItWasRead(Message message)
    {
        await hubContext.Clients.User(message.UserId.ToString()).ReceiveReadAt(message.Id, message.ReadAt!.Value);
    }

    public async Task NotifySupportUserMessageAuthorItWasSent(Message message)
    {
        var messageDto = mapper.Map<HubMessageDto>(message);
        await hubContext.Clients.User(message.UserId.ToString()).ReceiveMessageFromChat(message.ChatId, messageDto);
    }

    public async Task NotifySupportUsersOfTakenChat(Chat chat)
    {
        await hubContext.Clients.Group(ChatHub.SupportUserGroup).RemoveChatFromOpened(chat.Id);
    }
}

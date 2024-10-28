using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Support.Abstractions;
using Support.Data.Models;
using Support.Dtos;
using Support.Hubs;

namespace Support.Services;

public class ChatHubClientSenderService(
    IHubContext<ChatHub, IChatClient> hubContext,
    IClientNotificationProducer clientNotificationProducer,
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
        // TODO: Somehow check if client is online, and if they aren't:
        await clientNotificationProducer.NotifyClientOfNewMessage(message);
        // TODO: otherwise do nothing.
    }

    public async Task NotifyMessageAuthorItWasRead(Message message)
    {
        await hubContext.Clients.User(message.UserId.ToString()).ReceiveReadAt(message.Id, message.ReadAt!.Value);
    }

    public async Task NotifySupportUsersOfTakenChat(Chat chat)
    {
        await hubContext.Clients.Group(ChatHub.SupportUserGroup).RemoveChatFromOpened(chat.Id);
    }
}

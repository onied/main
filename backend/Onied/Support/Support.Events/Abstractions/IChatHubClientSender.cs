using Support.Data.Models;

namespace Support.Events.Abstractions;

public interface IChatHubClientSender
{
    Task SendMessageToSupportUsers(Message message);
    Task SendMessageToClient(Message message);
    Task NotifyMessageAuthorItWasRead(Message message);
    Task NotifySupportUserMessageAuthorItWasSent(Message message);
    Task NotifySupportUsersOfTakenChat(Chat chat);
}

using Support.Data.Models;

namespace Support.Abstractions;

public interface IChatHubClientSender
{
    Task SendMessageToSupportUsers(Message message);
    Task SendMessageToClient(Message message);
    Task NotifyMessageAuthorItWasRead(Message message);
    Task NotifySupportUsersOfTakenChat(Chat chat);
}

using Support.Dtos.Chat.GetChat.Response;

namespace Support.Abstractions;

public interface IChatService
{
    public Task<GetChatResponseDto> GetUserChat(Guid? userId);

    public Task<GetChatResponseDto> GetChatById(Guid chatId, Guid? userId);
}

using AutoMapper;
using Support.Abstractions;
using Support.Data.Abstractions;
using Support.Dtos.Chat.GetChat.Response;
using Support.Exceptions;

namespace Support.Services;

public class ChatService(
    IChatRepository chatRepository,
    IAuthorizationSupportUserService authorizationSupportUserService,
    ILogger<ChatService> logger,
    IMapper mapper) : IChatService
{
    public async Task<GetChatResponseDto> GetUserChat(Guid? userId)
    {
        if (userId is null)
        {
            logger.LogWarning("User ID is null. Cannot retrieve chat");
            throw new BadRequestException("userId query parameter cannot be null");
        }

        var chat = await chatRepository.GetWithSupportAndMessagesByUserIdAsync((Guid)userId);
        if (chat is null)
        {
            logger.LogWarning("No chat found for user ID: {UserId}. The chat has not yet been created", userId);
            throw new BadRequestException("The chat has not yet been created for this user");
        }

        logger.LogInformation("Retrieved chat for user {UserId}", userId);
        return mapper.Map<GetChatResponseDto>(chat);
    }

    public async Task<GetChatResponseDto> GetChatById(Guid chatId, Guid? userId)
    {
        await authorizationSupportUserService.AuthorizeSupportUser(userId);

        var chat = await chatRepository.GetWithSupportAndMessagesAsync(chatId);
        if (chat is null)
        {
            logger.LogWarning("Chat not found for Chat ID: {ChatId}", chatId);
            throw new NotFoundException($"The chat with ChatId = {chatId} was not found");
        }

        if (chat.Support != null)
        {
            logger.LogWarning("Attempt to access chat ID: {ChatId} which is already occupied by another support staff member", chatId);
            throw new ForbidException($"Chat with ChatId = {chatId} is already occupied by another support staff member");
        }

        logger.LogInformation("Retrieved chat for support staff member {UserId}", userId);
        return mapper.Map<GetChatResponseDto>(chat);
    }
}

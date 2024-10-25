using AutoMapper;
using Support.Abstractions;
using Support.Data.Abstractions;
using Support.Dtos.Support.GetChats.Response;
using Support.Dtos.Support.GetProfile.Response;

namespace Support.Services;

public class SupportService(
    IAuthorizationSupportUserService authorizationSupportUserService,
    IChatRepository chatRepository,
    ILogger<SupportService> logger,
    IMapper mapper) : ISupportService
{
    public async Task<List<GetChatsResponseDto>> GetActiveChats(Guid? userId)
    {
        await authorizationSupportUserService.AuthorizeSupportUser(userId);

        var chats = await chatRepository.GetActiveChatsAsync((Guid)userId!);
        logger.LogInformation("Retrieved {chatsCount} active chats for user {userId}", chats.Count, userId);
        return mapper.Map<List<GetChatsResponseDto>>(chats);
    }

    public async Task<List<GetChatsResponseDto>> GetOpenChats(Guid? userId)
    {
        await authorizationSupportUserService.AuthorizeSupportUser(userId);

        var chats = await chatRepository.GetOpenChatsAsync((Guid)userId!);
        logger.LogInformation("Retrieved {chatsCount} active chats for user {userId}", chats.Count, userId);
        return mapper.Map<List<GetChatsResponseDto>>(chats);
    }

    public async Task<GetProfileResponse> GetProfile(Guid? userId)
    {
        var supportUser = await authorizationSupportUserService.AuthorizeSupportUser(userId);
        return new GetProfileResponse { Number = supportUser.Number };
    }
}

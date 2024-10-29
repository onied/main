using Support.Dtos.Support.GetChats.Response;
using Support.Dtos.Support.GetProfile.Response;

namespace Support.Abstractions;

public interface ISupportService
{
    public Task<List<GetChatsResponseDto>> GetActiveChats(Guid? userId);

    public Task<List<GetChatsResponseDto>> GetOpenChats(Guid? userId);

    public Task<GetProfileResponse> GetProfile(Guid? userId);
}

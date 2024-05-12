using System.Security.Claims;
using Users.Dtos;

namespace Users.Services.ProfileService;

public interface IProfileService
{
    public Task<IResult> Get(ClaimsPrincipal claimsPrincipal);

    public Task<IResult> EditProfile(ProfileChangedDto profileChanged, ClaimsPrincipal claimsPrincipal);

    public Task<IResult> Avatar(AvatarChangedDto avatar, ClaimsPrincipal claimsPrincipal);
}

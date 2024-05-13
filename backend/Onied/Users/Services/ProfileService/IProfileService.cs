using System.Security.Claims;
using Users.Dtos.Profile.Request;

namespace Users.Services.ProfileService;

public interface IProfileService
{
    public Task<IResult> Get(ClaimsPrincipal claimsPrincipal);

    public Task<IResult> EditProfile(ProfileChangedRequest profileChanged, ClaimsPrincipal claimsPrincipal);

    public Task<IResult> Avatar(AvatarChangedRequest avatar, ClaimsPrincipal claimsPrincipal);
}

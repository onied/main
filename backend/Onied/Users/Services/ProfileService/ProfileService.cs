using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Users.Data.Entities;
using Users.Dtos.Profile.Request;
using Users.Dtos.Users.Response;
using Users.Services.ProfileProducer;

namespace Users.Services.ProfileService;

public class ProfileService(IProfileProducer profileProducer, UserManager<AppUser> userManager, IMapper mapper) : IProfileService
{
    public async Task<IResult> Get(ClaimsPrincipal claimsPrincipal)
    {
        var user = await userManager.GetUserAsync(claimsPrincipal);
        if (user == null)
            return Results.Unauthorized();
        var userProfile = mapper.Map<UserProfileResponse>(user);

        return Results.Ok(userProfile);
    }

    public async Task<IResult> EditProfile(ProfileChangedRequest profileChanged, ClaimsPrincipal claimsPrincipal)
    {
        var user = await userManager.GetUserAsync(claimsPrincipal);
        if (user == null)
            return Results.Unauthorized();
        user.FirstName = profileChanged.FirstName;
        user.LastName = profileChanged.LastName;
        user.Gender = profileChanged.Gender;
        await userManager.UpdateAsync(user);
        await profileProducer.PublishProfileUpdatedAsync(user);

        return Results.Ok();
    }

    public async Task<IResult> Avatar(AvatarChangedRequest avatar, ClaimsPrincipal claimsPrincipal)
    {
        var user = await userManager.GetUserAsync(claimsPrincipal);
        if (user == null)
            return Results.Unauthorized();
        user.Avatar = avatar.AvatarHref;
        await userManager.UpdateAsync(user);
        await profileProducer.PublishProfilePhotoUpdatedAsync(user);

        return Results.Ok();
    }
}

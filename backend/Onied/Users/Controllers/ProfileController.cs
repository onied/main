using Microsoft.AspNetCore.Mvc;
using Users.Dtos;
using Users.Services.ProfileService;

namespace Users.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ProfileController(IProfileService profileService) : ControllerBase
{
    [HttpGet]
    public Task<IResult> Get()
    {
        return profileService.Get(User);
    }

    [HttpPut]
    public Task<IResult> EditProfile([FromBody] ProfileChangedDto profileChanged)
    {
        return profileService.EditProfile(profileChanged, User);
    }

    [HttpPut("avatar")]
    public Task<IResult> Avatar([FromBody] AvatarChangedDto avatar)
    {
        return profileService.Avatar(avatar, User);
    }
}

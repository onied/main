using MediatR;
using Microsoft.AspNetCore.Mvc;
using Users.Commands;
using Users.Dtos.Profile.Request;
using Users.Queries;
using Users.Services.ProfileService;

namespace Users.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ProfileController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<IResult> Get()
    {
        return await sender.Send(new GetProfileQuery(User));
    }

    [HttpPut]
    public async Task<IResult> EditProfile([FromBody] ProfileChangedRequest profileChanged)
    {
        return await sender.Send(new EditProfileCommand(profileChanged, User));
    }

    [HttpPut("avatar")]
    public async Task<IResult> Avatar([FromBody] AvatarChangedRequest avatar)
    {
        return await sender.Send(new EditProfileAvatarCommand(avatar, User));
    }
}

using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Users.Dtos;

namespace Users.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ProfileController : ControllerBase
{
    private readonly ILogger<ProfileController> _logger;
    private readonly IMapper _mapper;

    public ProfileController(ILogger<ProfileController> logger, IMapper mapper)
    {
        _logger = logger;
        _mapper = mapper;
    }

   [HttpGet]
    public async Task<ActionResult<UserProfileDto>> Get([FromServices] UserManager<AppUser> userManager)
    {
        var user = await userManager.GetUserAsync(User);
        if (user == null)
            return Unauthorized();
        var userProfile = _mapper.Map<UserProfileDto>(user);

        return Ok(userProfile);
    }

    [HttpPut("")]
    public async Task<ActionResult> EditProfile(
        [FromServices] UserManager<AppUser> userManager,
        [FromBody] ProfileChangedDto profileChanged)
    {
        var user = await userManager.GetUserAsync(User);
        if (user == null)
            return Unauthorized();
        user.FirstName = profileChanged.FirstName;
        user.LastName = profileChanged.LastName;
        user.Gender = profileChanged.Gender;
        await userManager.UpdateAsync(user);

        return Ok();
    }

    [HttpPut("avatar")]
    public async Task<ActionResult> Avatar(
        [FromServices] UserManager<AppUser> userManager,
        [FromBody] AvatarChangedDto avatar)
    {
        var user = await userManager.GetUserAsync(User);
        if (user == null)
            return Unauthorized();
        user.Avatar = avatar.AvatarHref;
        await userManager.UpdateAsync(user);

        return Ok();
    }
}

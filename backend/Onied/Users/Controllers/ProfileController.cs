using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Users.Data;
using Users.Dtos;
using Users.Services.ProfileProducer;

namespace Users.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ProfileController : ControllerBase
{
    private readonly ILogger<ProfileController> _logger;
    private readonly IMapper _mapper;
    private readonly IProfileProducer _profileProducer;

    public ProfileController(
        ILogger<ProfileController> logger,
        IMapper mapper,
        IProfileProducer profileProducer)
    {
        _logger = logger;
        _mapper = mapper;
        _profileProducer = profileProducer;
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

    [HttpPut]
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
        await _profileProducer.PublishProfileUpdatedAsync(user);

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
        await _profileProducer.PublishProfilePhotoUpdatedAsync(user);

        return Ok();
    }
}

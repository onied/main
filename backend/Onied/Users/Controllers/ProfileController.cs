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
    private readonly AppDbContext _context;
    private readonly ILogger<ProfileController> _logger;
    private readonly IMapper _mapper;

    public ProfileController(AppDbContext context, ILogger<ProfileController> logger, IMapper mapper)
    {
        _context = context;
        _logger = logger;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<UserProfileDto>> Get([FromServices] SignInManager<AppUser> signInManager)
    {
        var currentUser = HttpContext.User;

        if (!signInManager.IsSignedIn(currentUser))
        {
            return Unauthorized();
        }

        var userId = currentUser.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _context.FindAsync<AppUser>(userId);
        var userProfile = _mapper.Map<UserProfileDto>(user);

        return Ok(userProfile);
    }

    [HttpPut("")]
    public async Task<ActionResult> EditProfile(
        [FromServices] SignInManager<AppUser> signInManager,
        [FromBody] ProfileChangedDto profileChanged)
    {
        var currentUser = User;

        if (!signInManager.IsSignedIn(currentUser))
        {
            return Unauthorized();
        }

        var userId = currentUser.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _context.FindAsync<AppUser>(userId);
        user.FirstName = profileChanged.FirstName;
        user.LastName = profileChanged.LastName;
        user.Gender = profileChanged.Gender;
        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpPut("avatar")]
    public async Task<ActionResult> Avatar(
        [FromServices] SignInManager<AppUser> signInManager,
        [FromBody] AvatarChangedDto avatar)
    {
        var currentUser = User;

        if (!signInManager.IsSignedIn(currentUser))
        {
            return Unauthorized();
        }

        var userId = currentUser.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _context.FindAsync<AppUser>(userId);
        user.Avatar = avatar.AvatarHref;
        await _context.SaveChangesAsync();

        return Ok();
    }
}

using System.Security.Claims;
using AutoMapper;
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
}

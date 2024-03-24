using Courses.Models;
using Courses.Services;
using Microsoft.AspNetCore.Mvc;

namespace Courses.Controllers;

[ApiController]
[Route("api/v1/[controller]/{id:guid}")]
public class AccountsController(IUserRepository userRepository) : ControllerBase
{
    [HttpGet]
    [Route("courses")]
    public async Task<ActionResult<List<Course>>> GetCourses(Guid id)
    {
        var user = await userRepository.GetUserWithCoursesAsync(id);
        if (user is null) return NotFound();

        return user.Courses.ToList();
    }
}

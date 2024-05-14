using AutoMapper;
using Courses.Dtos;
using Courses.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Courses.Controllers;

[ApiController]
[Route("api/v1/[controller]/{id:guid}")]
public class AccountsController(IUserRepository userRepository, IMapper mapper) : ControllerBase
{
    [HttpGet]
    [Route("courses")]
    public async Task<ActionResult<List<CourseCardDto>>> GetCourses(Guid id)
    {
        var user = await userRepository.GetUserWithCoursesAsync(id);
        if (user is null) return NotFound();

        var courses = mapper.Map<List<CourseCardDto>>(user.Courses);
        foreach (var course in courses)
            course.IsOwned = true;

        return courses;
    }
}

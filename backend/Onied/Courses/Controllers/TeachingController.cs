using AutoMapper;
using Courses.Dtos;
using Courses.Services;
using Microsoft.AspNetCore.Mvc;

namespace Courses.Controllers;

[ApiController]
[Route("api/v1/[controller]/{id:guid}")]
public class TeachingController(
    UserRepository userRepository,
    IMapper mapper
    ) : ControllerBase
{
    [HttpGet]
    [Route("authored")]
    public async Task<ActionResult<List<CourseCardDto>>> GetAuthoredCourses(Guid id)
    {
        var user = await userRepository.GetUserWithTeachingCoursesAsync(id);
        if (user is null) return NotFound();

        return mapper.Map<List<CourseCardDto>>(user.Courses);
    }

    [HttpGet]
    [Route("moderated")]
    public async Task<ActionResult<List<CourseCardDto>>> GetModeratedCourses(Guid id)
    {
        var user = await userRepository.GetUserWithTeachingCoursesAsync(id);
        if (user is null) return NotFound();

        return mapper.Map<List<CourseCardDto>>(user.Courses);
    }
}

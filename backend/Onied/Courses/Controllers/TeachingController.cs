using AutoMapper;
using Courses.Dtos;
using Courses.Services;
using Courses.Services.Abstractions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Courses.Controllers;

[ApiController]
[Route("api/v1/[controller]/{id:guid}")]
public class TeachingController(IUserRepository userRepository, IMapper mapper) : ControllerBase
{
    [HttpGet]
    [Route("authored")]
    public async Task<Results<Ok<List<CourseCardDto>>, NotFound>> GetAuthoredCourses(Guid id)
    {
        var user = await userRepository.GetUserWithTeachingCoursesAsync(id);
        if (user is null) return TypedResults.NotFound();

        return TypedResults.Ok(mapper.Map<List<CourseCardDto>>(user.TeachingCourses));
    }

    [HttpGet]
    [Route("moderated")]
    public async Task<Results<Ok<List<CourseCardDto>>, NotFound>> GetModeratedCourses(Guid id)
    {
        var user = await userRepository.GetUserWithModeratingCoursesAsync(id);
        if (user is null) return TypedResults.NotFound();

        return TypedResults.Ok(mapper.Map<List<CourseCardDto>>(user.ModeratingCourses));
    }
}

using AutoMapper;
using Courses.Dtos;
using Courses.Helpers;
using Courses.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Courses.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class CatalogController(
    ILogger<CatalogController> logger,
    IMapper mapper,
    ICourseRepository courseRepository,
    IUserRepository userRepository)
    : ControllerBase
{
    [HttpGet]
    public async Task<Page<CourseCardDto>> Get(
        [FromQuery] CatalogGetQueriesDto catalogGetQueries,
        [FromQuery] Guid? userId)
    {
        var (courses, count) = await courseRepository.GetCoursesAsync(catalogGetQueries);
        var courseDtos = mapper.Map<List<CourseCardDto>>(courses);

        var userCourses = (userId is null
                ? null
                : await userRepository.GetUserWithCoursesAsync(userId.Value))?.Courses
            .Select(x => x.Id).ToList() ?? [];
        courseDtos.ForEach(x => x.IsOwned = userCourses.Contains(x.Id));

        return Page<CourseCardDto>.Prepare(catalogGetQueries, count, courseDtos);
    }
}

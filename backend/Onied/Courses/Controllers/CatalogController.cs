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
        [FromQuery] PageQuery pageQuery,
        [FromQuery] Guid? userId)
    {
        var page = Page<CourseCardDto>.Prepare(
            pageQuery,
            await courseRepository.CountAsync(),
            out var offset);
        var courses = await courseRepository.GetCoursesAsync(
            offset,
            page.ElementsPerPage);

        var courseDtos = mapper.Map<List<CourseCardDto>>(courses);

        var userCourses = (userId is null
                ? null
                : await userRepository.GetUserWithCoursesAsync(userId.Value))?.Courses
            .Select(x => x.Id).ToList() ?? [];
        courseDtos.ForEach(x => x.IsOwned = userCourses.Contains(x.Id));

        page.Elements = courseDtos;
        return page;
    }
}

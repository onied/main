using AutoMapper;
using Courses.Dtos;
using Courses.Helpers;
using Courses.Services.Abstractions;

namespace Courses.Services;

public class CatalogService(
    ICourseRepository courseRepository,
    IUserRepository userRepository,
    IMapper mapper) : ICatalogService
{
    public async Task<IResult> Get(
        CatalogGetQueriesDto catalogGetQueries,
        Guid? userId)
    {
        var (courses, count) = await courseRepository.GetCoursesAsync(catalogGetQueries);
        var courseDtos = mapper.Map<List<CourseCardDto>>(courses);

        var userCourses = (userId is null
                ? null
                : await userRepository.GetUserWithCoursesAsync(userId.Value))?.Courses
            .Select(x => x.Id).ToList() ?? [];
        courseDtos.ForEach(x => x.IsOwned = userCourses.Contains(x.Id));

        return Results.Ok(Page<CourseCardDto>.Prepare(catalogGetQueries, count, courseDtos));
    }
}

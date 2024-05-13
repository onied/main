using AutoMapper;
using Courses.Dtos;
using Courses.Services.Abstractions;

namespace Courses.Services;

public class TeachingService(IUserRepository userRepository, IMapper mapper) : ITeachingService
{
    public async Task<IResult> GetAuthoredCourses(Guid userId)
    {
        var user = await userRepository.GetUserWithTeachingCoursesAsync(userId);
        if (user is null) return Results.NotFound();

        return Results.Ok(mapper.Map<List<CourseCardDto>>(user.TeachingCourses));
    }

    public async Task<IResult> GetModeratedCourses(Guid userId)
    {
        var user = await userRepository.GetUserWithModeratingCoursesAsync(userId);
        if (user is null) return Results.NotFound();

        return Results.Ok(mapper.Map<List<CourseCardDto>>(user.ModeratingCourses));
    }
}

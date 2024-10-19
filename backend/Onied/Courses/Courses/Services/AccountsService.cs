using AutoMapper;
using Courses.Dtos;
using Courses.Dtos.Catalog.Response;
using Courses.Services.Abstractions;

namespace Courses.Services;

public class AccountsService(IUserRepository userRepository, IMapper mapper) : IAccountsService
{
    public async Task<IResult> GetCourses(Guid id)
    {
        var user = await userRepository.GetUserWithCoursesAsync(id);
        if (user is null) return Results.NotFound();

        var courses = mapper.Map<List<CourseCardResponse>>(user.Courses);
        foreach (var course in courses)
            course.IsOwned = true;

        return Results.Ok(courses);
    }
}

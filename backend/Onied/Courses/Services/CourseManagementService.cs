using Courses.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Courses.Services;

public class CourseManagementService(ICourseRepository courseRepository) : ICourseManagementService
{
    public async Task<Results<Ok<Course>, NotFound, ForbidHttpResult>> CheckCourseAuthorAsync(int courseId, string? userId)
    {
        var course = await courseRepository.GetCourseAsync(courseId);
        if (course == null)
            return TypedResults.NotFound();
        if (userId == null || course.Author?.Id.ToString() != userId)
            return TypedResults.Forbid();
        return TypedResults.Ok(course);
    }
}

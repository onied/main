using Courses.Models;
using Courses.Services.Abstractions;
using Microsoft.AspNetCore.Http.HttpResults;
using OneOf;

namespace Courses.Services;

public class CourseManagementService(ICourseRepository courseRepository) : ICourseManagementService
{
    public async Task<OneOf<Ok<Course>, NotFound, ForbidHttpResult>> CheckCourseAuthorAsync(int courseId,
        string? userId)
    {
        var course = await courseRepository.GetCourseAsync(courseId);
        if (course == null)
            return TypedResults.NotFound();
        if (userId == null || course.Author?.Id.ToString() != userId)
            return TypedResults.Forbid();
        return TypedResults.Ok(course);
    }
}

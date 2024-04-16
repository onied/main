using Courses.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Courses.Services.Abstractions;

public interface ICourseManagementService
{
    public Task<Results<Ok<Course>, NotFound, ForbidHttpResult>> CheckCourseAuthorAsync(int courseId, string? userId);
    public Task<bool> AllowVisitCourse(Guid userId, int courseId);
}

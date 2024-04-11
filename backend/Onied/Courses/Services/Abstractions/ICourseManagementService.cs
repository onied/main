using Courses.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using OneOf;

namespace Courses.Services.Abstractions;

public interface ICourseManagementService
{
    public Task<OneOf<Ok<Course>, NotFound, ForbidHttpResult>> CheckCourseAuthorAsync(int courseId, string? userId);
}

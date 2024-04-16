using Courses.Dtos.ModeratorDtos.Response;
using Courses.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Courses.Services.Abstractions;

public interface ICourseManagementService
{
    public Task<Results<Ok<Course>, NotFound, ForbidHttpResult>> CheckCourseAuthorAsync(int courseId, string? userId);
    public Task<Results<Ok<CourseStudentsDto>, NotFound, ForbidHttpResult>> GetStudents(int courseId, Guid authorId);
    public Task<Results<Ok, NotFound<string>, ForbidHttpResult>> DeleteModerator(int courseId, Guid studentId, Guid authorId);
    public Task<Results<Ok, NotFound<string>, ForbidHttpResult>> AddModerator(int courseId, Guid studentId, Guid authorId);
}

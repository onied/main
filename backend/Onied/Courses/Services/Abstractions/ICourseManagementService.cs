using Courses.Dtos;
using Courses.Dtos.Course.Response;
using Courses.Dtos.EditCourse.Request;
using Courses.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Courses.Services.Abstractions;

public interface ICourseManagementService
{
    public Task<IResult> CheckCourseAuthorAsync(int courseId, string? userId);
    public Task<bool> AllowVisitCourse(Guid userId, int courseId);
    public Task<IResult> GetStudents(int courseId, Guid authorId);
    public Task<IResult> DeleteModerator(int courseId, Guid studentId, Guid authorId);
    public Task<IResult> AddModerator(int courseId, Guid studentId, Guid authorId);

    public Task<IResult> EditTasksBlock(
        int id,
        int blockId,
        string? userId,
        EditTasksBlockRequest tasksBlockRequest);

    public Task<IResult> EditSummaryBlock(
        int id,
        int blockId,
        string? userId,
        SummaryBlockResponse summaryBlockResponse);

    public Task<IResult> EditVideoBlock(
        int id,
        int blockId,
        string? userId,
        VideoBlockResponse videoBlockResponse);

    public Task<IResult> RenameBlock(
        int id,
        RenameBlockRequest renameBlockRequest,
        string? userId);

    public Task<IResult> DeleteBlock(
        int id,
        int blockId,
        string? userId);

    public Task<IResult> AddBlock(
        int id,
        int moduleId,
        int blockType,
        string? userId);

    public Task<IResult> RenameModule(
        int id,
        string? userId,
        RenameModuleRequest renameModuleRequest);

    public Task<IResult> DeleteModule(
        int id,
        int moduleId,
        string? userId);

    public Task<IResult> AddModule(
        int id,
        string? userId);

    public Task<IResult> EditHierarchy(
        int id,
        string? userId,
        CourseResponse courseResponse);

    public Task<IResult> EditCourse(int id,
        string? userId,
        EditCourseRequest editCourseRequest);


}

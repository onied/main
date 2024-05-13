using Courses.Dtos;
using Courses.Dtos.ModeratorDtos.Response;
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
        EditTasksBlockDto tasksBlockDto);

    public Task<IResult> EditSummaryBlock(
        int id,
        int blockId,
        string? userId,
        SummaryBlockDto summaryBlockDto);

    public Task<IResult> EditVideoBlock(
        int id,
        int blockId,
        string? userId,
        VideoBlockDto videoBlockDto);

    public Task<IResult> RenameBlock(
        int id,
        RenameBlockDto renameBlockDto,
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
        RenameModuleDto renameModuleDto);

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
        CourseDto courseDto);

    public Task<IResult> EditCourse(int id,
        string? userId,
        EditCourseDto editCourseDto);


}

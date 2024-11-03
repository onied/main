using Courses.Dtos.Course.Response;
using Courses.Dtos.EditCourse.Request;

namespace Courses.Services.Abstractions;

public interface ICourseManagementService
{
    public Task<IResult> CheckCourseAuthorAsync(int courseId, string? userId,
        string? role);
    public Task<bool> AllowVisitCourse(Guid userId, int courseId, string? role = null);
    public Task<IResult> GetStudents(int courseId, Guid authorId);
    public Task<IResult> DeleteModerator(int courseId, Guid studentId, Guid authorId);
    public Task<IResult> AddModerator(int courseId, Guid studentId, Guid authorId);

    public Task<IResult> EditTasksBlock(int id, int blockId, EditTasksBlockRequest tasksBlockRequest);

    public Task<IResult> EditSummaryBlock(int id, int blockId, SummaryBlockResponse summaryBlockResponse);

    public Task<IResult> EditVideoBlock(int id, int blockId, VideoBlockResponse videoBlockResponse);

    public Task<IResult> RenameBlock(int id, RenameBlockRequest renameBlockRequest);

    public Task<IResult> DeleteBlock(int id, int blockId);

    public Task<IResult> AddBlock(int id, int moduleId, int blockType);

    public Task<IResult> RenameModule(int id, RenameModuleRequest renameModuleRequest);

    public Task<IResult> DeleteModule(int id, int moduleId);

    public Task<IResult> AddModule(int id);

    public Task<IResult> EditHierarchy(int id, CourseResponse courseResponse);

    public Task<IResult> EditCourse(int id, EditCourseRequest editCourseRequest, string userId);


}

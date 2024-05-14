using Courses.Dtos;
using Courses.Dtos.CheckTasks.Request;
using Courses.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Task = System.Threading.Tasks.Task;

namespace Courses.Services.Abstractions;

public interface ICheckTaskManagementService
{
    public Task<IResult> TryGetTaskBlock(
        Guid userId, int courseId, int blockId,
        bool includeVariants = false,
        bool includeAnswers = false);

    public IResult GetUserTaskPoints(
        List<UserInputRequest> inputsDto,
        TasksBlock block,
        Guid userId);

    public Task ManageTaskBlockCompleted(
        List<UserTaskPoints> pointsInfo,
        Guid userId,
        int blockId);

    public Task ManageCourseCompleted(Guid userId, int courseId);

    public Task<IResult> GetTaskPointsStored(
        int courseId,
        int blockId,
        Guid userId);

    public Task<IResult> CheckTaskBlock(
        int courseId,
        int blockId,
        Guid userId,
        List<UserInputRequest> inputsDto);
}

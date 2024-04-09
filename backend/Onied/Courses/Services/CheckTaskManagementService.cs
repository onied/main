using System.Collections.Immutable;
using Courses.Dtos;
using Courses.Models;
using Courses.Services.Abstractions;
using Microsoft.AspNetCore.Http.HttpResults;
using Task = System.Threading.Tasks.Task;

namespace Courses.Services;

public class CheckTaskManagementService(
    IBlockRepository blockRepository,
    IBlockCompletedInfoRepository blockCompletedInfoRepository,
    IUserCourseInfoRepository userCourseInfoRepository,
    UserTaskPointsRepository userTaskPointsRepository,
    ICheckTasksService checkTasksService)
    : ICheckTaskManagementService
{
    public async Task<Results<Ok<TasksBlock>, NotFound, ForbidHttpResult>> TryGetTaskBlock(
        Guid userId, int courseId, int blockId,
        bool includeVariants = false,
        bool includeAnswers = false)
    {
        var block = await blockRepository.GetTasksBlock(blockId, includeVariants, includeAnswers);

        if (block == null || block.Module.CourseId != courseId)
            return TypedResults.NotFound();

        if (await userCourseInfoRepository.GetUserCourseInfoAsync(userId, courseId) is null)
            return TypedResults.Forbid();

        return TypedResults.Ok(block);
    }

    public Results<Ok<List<UserTaskPoints?>>, NotFound<string>, BadRequest<string>> GetUserTaskPoints(
        List<UserInputDto> inputsDto,
        TasksBlock block,
        Guid userId)
    {
        var points = new List<UserTaskPoints?>();
        foreach (var inputDto in inputsDto)
        {
            var task = block.Tasks.SingleOrDefault(task => inputDto.TaskId == task.Id);

            if (task is null)
                return TypedResults.NotFound($"Task with id={inputDto.TaskId} not found.");

            if (task.TaskType != inputDto.TaskType)
                return TypedResults.BadRequest(
                    $"Task with id={inputDto.TaskId} has invalid TaskType={inputDto.TaskType}.");

            var tp = checkTasksService.CheckTask(task, inputDto);
            tp.UserId = userId;
            tp.CourseId = block.Module.CourseId;
            points.Add(tp);
        }

        return TypedResults.Ok(points);
    }

    public async Task ManageTaskBlockCompleted(List<UserTaskPoints> pointsInfo, Guid userId, int blockId)
    {
        var bci = await blockCompletedInfoRepository
            .GetCompletedCourseBlockAsync(userId, blockId);
        if (pointsInfo.All(utp => utp.HasFullPoints) && bci is null)
        {
            await blockCompletedInfoRepository.AddCompletedCourseBlockAsync(userId, blockId);
        }
        else if (pointsInfo.Any(utp => !utp.HasFullPoints)
                 && bci is not null)
        {
            await blockCompletedInfoRepository.DeleteCompletedCourseBlocksAsync(bci);
        }
    }
}

using AutoMapper;
using Courses.Dtos;
using Courses.Models;
using Courses.Services.Abstractions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Courses.Controllers;

[Route("api/v1/courses/{courseId:int}/tasks/{blockId:int}")]
public class CheckTasksController(
    ILogger<CoursesController> logger,
    IMapper mapper,
    IUserTaskPointsRepository userTaskPointsRepository,
    ICheckTaskManagementService checkTaskManagementService) : ControllerBase
{
    [HttpGet]
    [Route("points")]
    public async Task<Results<Ok<List<UserTaskPointsDto>>, NotFound, ForbidHttpResult>> GetTaskPointsStored(
        int courseId,
        int blockId,
        [FromQuery] Guid userId)
    {
        var response = await checkTaskManagementService
            .TryGetTaskBlock(userId, courseId, blockId, true);
        if (response.Result is not Ok<TasksBlock> ok)
            return (dynamic)response.Result;
        var block = ok.Value!;

        var storedPoints = (await userTaskPointsRepository
                .GetUserTaskPointsByUserAndBlock(userId, courseId, blockId))
            .Where(utp => utp.Checked).ToList();

        var points = block.Tasks.Select(
            task => storedPoints.SingleOrDefault(tp => tp.TaskId == task.Id)
                    ?? (task.TaskType is TaskType.ManualReview
                        ? null
                        : new UserTaskPoints
                        {
                            UserId = userId,
                            TaskId = task.Id,
                            CourseId = courseId,
                            Points = 0,
                            Checked = false
                        })
        ).ToList();

        return TypedResults.Ok(mapper.Map<List<UserTaskPointsDto>>(points));
    }

    [HttpPost]
    [Route("check")]
    public async Task<Results<Ok<List<UserTaskPointsDto>>, NotFound<string>, ForbidHttpResult, BadRequest<string>>>
        CheckTaskBlock(
            int courseId,
            int blockId,
            [FromQuery] Guid userId,
            [FromBody] List<UserInputDto> inputsDto)
    {
        var responseGetTaskBlock = await checkTaskManagementService
            .TryGetTaskBlock(userId, courseId, blockId, true, true);
        if (responseGetTaskBlock.Result is not Ok<TasksBlock> okGetTaskBlock)
            return (dynamic)responseGetTaskBlock.Result;

        var block = okGetTaskBlock.Value!;

        var responseGeTaskPoints = checkTaskManagementService
            .GetUserTaskPoints(inputsDto, block, userId);
        if (responseGetTaskBlock.Result is not Ok<List<UserTaskPoints>> okGetTaskPoints)
            return (dynamic)responseGeTaskPoints.Result;

        var pointsInfo = okGetTaskPoints.Value!;

        await userTaskPointsRepository
            .StoreUserTaskPointsForConcreteUserAndBlock(
                pointsInfo, userId, courseId, blockId);
        await checkTaskManagementService
            .ManageTaskBlockCompleted(pointsInfo, userId, blockId);

        var pointsPrepared = block.Tasks
            .Select(task => task.TaskType is TaskType.ManualReview
                ? null
                : pointsInfo.SingleOrDefault(tp => tp.TaskId == task.Id));

        return TypedResults.Ok(mapper.Map<List<UserTaskPointsDto>>(pointsPrepared));
    }
}

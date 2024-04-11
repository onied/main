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
        if (!response.TryPickT0(out var ok, out var remainder))
            return remainder.Match<Results<Ok<List<UserTaskPointsDto>>, NotFound, ForbidHttpResult>>(
                notFound => notFound, forbid => forbid);
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
        if (!responseGetTaskBlock.TryPickT0(out var okGetTaskBlock, out var remainderGetTaskBlock))
            return remainderGetTaskBlock
                .Match<Results<Ok<List<UserTaskPointsDto>>, NotFound<string>, ForbidHttpResult, BadRequest<string>>>(
                    _ => TypedResults.NotFound("Task block not found."), forbid => forbid);

        var block = okGetTaskBlock.Value!;

        var responseGetTaskPoints = checkTaskManagementService
            .GetUserTaskPoints(inputsDto, block, userId);
        if (!responseGetTaskPoints.TryPickT0(out var okGetTaskPoints, out var remainderGetTaskPoints))
            return remainderGetTaskPoints
                .Match<Results<Ok<List<UserTaskPointsDto>>, NotFound<string>, ForbidHttpResult, BadRequest<string>>>(
                    notFound => notFound, badRequest => badRequest);

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

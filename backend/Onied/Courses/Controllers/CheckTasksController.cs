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
    ICourseManagementService courseManagementService,
    ICheckTaskManagementService checkTaskManagementService) : ControllerBase
{
    [HttpGet]
    [Route("points")]
    public async Task<Results<Ok<List<UserTaskPointsDto>>, NotFound, ForbidHttpResult>> GetTaskPointsStored(
        int courseId,
        int blockId,
        [FromQuery] Guid userId)
    {
        if (!await courseManagementService.AllowVisitCourse(userId, courseId))
            return TypedResults.Forbid();

        var response = await checkTaskManagementService
            .TryGetTaskBlock(userId, courseId, blockId, true);
        if (response.Result is not Ok<TasksBlock> ok)
            return (dynamic)response.Result;
        var block = ok.Value!;

        var storedPoints = (await userTaskPointsRepository
            .GetUserTaskPointsByUserAndBlock(userId, courseId, blockId)).ToList();

        var points = block.Tasks.Select(task =>
            {
                var points = storedPoints.SingleOrDefault(tp => tp.TaskId == task.Id);
                if (points == null)
                    return new UserTaskPointsDto
                    {
                        TaskId = task.Id,
                        Points = null
                    };
                var dto = mapper.Map<UserTaskPointsDto>(points);
                if (!points.Checked)
                    dto.Points = null;
                if (points is ManualReviewTaskUserAnswer manualReviewTaskUserAnswer)
                    dto.Content = manualReviewTaskUserAnswer.Content;
                return dto;
            }
        ).ToList();

        return TypedResults.Ok(points);
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
        if (!await courseManagementService.AllowVisitCourse(userId, courseId))
            return TypedResults.Forbid();

        var responseGetTaskBlock = await checkTaskManagementService
            .TryGetTaskBlock(userId, courseId, blockId, true, true);
        if (responseGetTaskBlock.Result is not Ok<TasksBlock> okGetTaskBlock)
            return (dynamic)responseGetTaskBlock.Result;

        var block = okGetTaskBlock.Value!;

        var responseGetTaskPoints = checkTaskManagementService
            .GetUserTaskPoints(inputsDto, block, userId);
        if (responseGetTaskPoints.Result is not Ok<List<UserTaskPoints>> okGetTaskPoints)
            return (dynamic)responseGetTaskPoints.Result;

        var pointsInfo = okGetTaskPoints.Value!;

        await userTaskPointsRepository
            .StoreUserTaskPointsForConcreteUserAndBlock(
                pointsInfo, userId, courseId, blockId);
        await checkTaskManagementService
            .ManageTaskBlockCompleted(pointsInfo, userId, blockId);
        await checkTaskManagementService
            .ManageCourseCompleted(userId, courseId);

        var pointsPrepared = block.Tasks
            .Select(task =>
            {
                var points = pointsInfo.SingleOrDefault(tp => tp.TaskId == task.Id);
                if (points == null)
                    return new UserTaskPointsDto
                    {
                        TaskId = task.Id,
                        Points = null
                    };
                var dto = mapper.Map<UserTaskPointsDto>(points);
                if (!points.Checked)
                    dto.Points = null;
                if (points is ManualReviewTaskUserAnswer manualReviewTaskUserAnswer)
                    dto.Content = manualReviewTaskUserAnswer.Content;
                return dto;
            }).ToList();

        return TypedResults.Ok(pointsPrepared);
    }
}

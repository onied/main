using AutoMapper;
using Courses.Dtos;
using Courses.Models;
using Courses.Services;
using Courses.Services.Abstractions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Courses.Controllers;

[Route("api/v1/courses/{courseId:int}/tasks/{blockId:int}")]
public class CheckTasksController(
    ILogger<CoursesController> logger,
    IMapper mapper,
    ICheckTasksService checkTasksService,
    IUserTaskPointsRepository userTaskPointsRepository,
    ICheckTaskManagementService checkTaskManagementService,
    IBlockCompletedInfoRepository blockCompletedInfoRepository) : ControllerBase
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

        var storedPoints = await userTaskPointsRepository
            .GetUserTaskPointsByUserAndBlock(userId, courseId, blockId);

        var points = block.Tasks.Select(
            task => storedPoints.SingleOrDefault(tp => tp.TaskId == task.Id)
                    ?? (task.TaskType is TaskType.ManualReview
                        ? null
                        : new UserTaskPoints
                        {
                            UserId = userId,
                            TaskId = task.Id,
                            CourseId = courseId,
                            Points = 0
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
        var response = await checkTaskManagementService
            .TryGetTaskBlock(userId, courseId, blockId, true, true);
        if (response.Result.GetType() != typeof(Ok<TasksBlock>))
            return (dynamic)response.Result;
        var block = ((Ok<TasksBlock>)response.Result).Value!;

        var points = new List<UserTaskPoints?>();
        foreach (var inputDto in inputsDto)
        {
            var task = block.Tasks.SingleOrDefault(task => inputDto.TaskId == task.Id);

            if (task is null)
                return TypedResults.NotFound($"Task with id={inputDto.TaskId} not found.");

            if (task.TaskType != inputDto.TaskType)
                return TypedResults.BadRequest(
                    $"Task with id={inputDto.TaskId} has invalid TaskType={inputDto.TaskType}.");

            points.Add(checkTasksService.CheckTask(task, inputDto));
        }

        var pointsInfo = points.Where(tp => tp != null)
            .Select(tp =>
            {
                tp!.UserId = userId;
                return tp;
            })
            .ToList();
        await userTaskPointsRepository.StoreUserTaskPointsForConcreteUserAndBlock(
            pointsInfo, userId, courseId, blockId);

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

        return TypedResults.Ok(mapper.Map<List<UserTaskPointsDto>>(points));
    }
}

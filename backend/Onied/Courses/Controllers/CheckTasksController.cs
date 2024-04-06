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
    IBlockRepository blockRepository,
    ICheckTasksService checkTasksService,
    IUserCourseInfoRepository userCourseInfoRepository,
    IStoreUserTaskPointsService storeUserTaskPointsService) : ControllerBase
{
    [HttpGet]
    [Route("points")]
    public async Task<Results<Ok<List<UserTaskPointsDto>>, NotFound, ForbidHttpResult>> GetTaskPointsStored(
        int courseId,
        int blockId,
        [FromQuery] Guid userId)
    {
        var block = await blockRepository.GetTasksBlock(blockId, true);

        if (block == null || block.Module.CourseId != courseId)
            return TypedResults.NotFound();

        var storedPoints = await storeUserTaskPointsService.GetUserTaskPointsByBlock(userId, courseId, blockId);
        if (storedPoints is null) return TypedResults.Forbid();

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
    public async Task<ActionResult<List<UserTaskPointsDto>>> CheckTaskBlock(
        int courseId,
        int blockId,
        [FromQuery] Guid userId,
        [FromBody] List<UserInputDto> inputsDto)
    {
        if (inputsDto.Any(inputDto => inputDto is null)) return BadRequest();

        var block = await blockRepository.GetTasksBlock(blockId, true, true);
        if (block is null || block.Module.CourseId != courseId) return NotFound();

        var userCourseInfo = await userCourseInfoRepository.GetUserCourseInfoAsync(userId, courseId);
        if (userCourseInfo is null) return Forbid();

        var points = new List<UserTaskPoints?>();
        foreach (var inputDto in inputsDto)
        {
            var task = block.Tasks.SingleOrDefault(task => inputDto.TaskId == task.Id);

            if (task is null)
                return NotFound($"Task with id={inputDto.TaskId} not found.");

            if (task.TaskType != inputDto.TaskType)
                return BadRequest($"Task with id={inputDto.TaskId} has invalid TaskType={inputDto.TaskType}.");

            points.Add(checkTasksService.CheckTask(task, inputDto));
        }

        await storeUserTaskPointsService.StoreUserTaskPoints(
            userCourseInfo,
            points.Where(tp => tp != null)
                .Select(tp => tp!)
                .ToList());

        return mapper.Map<List<UserTaskPointsDto>>(points);
    }
}

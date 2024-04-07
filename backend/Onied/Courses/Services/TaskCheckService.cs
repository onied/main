using AutoMapper;
using Courses.Dtos.TaskCheckDtos;
using Courses.Dtos.TaskCheckDtos.Request;
using Courses.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Courses.Services;

public class TaskCheckService(IUserRepository userRepository, ITaskCheckRepository taskCheckRepository, IMapper mapper)
    : ITaskCheckService
{
    private async Task<Results<Ok<TaskCheck>, NotFound, UnauthorizedHttpResult, ForbidHttpResult>> GetTaskToCheck(
        Guid userId,
        Guid taskCheckId)
    {
        var user = await userRepository.GetUserWithModeratingAndTeachingCoursesAsync(userId);
        if (user == null)
            return TypedResults.Unauthorized();
        var taskCheck = await taskCheckRepository.GetTaskCheck(taskCheckId);
        if (taskCheck == null)
            return TypedResults.NotFound();
        if (!taskCheckRepository.CanCheckTask(user, taskCheck))
            return TypedResults.Forbid();
        return TypedResults.Ok(taskCheck);
    }

    public async Task<Results<Ok<TaskCheckDto>, NotFound, UnauthorizedHttpResult, ForbidHttpResult>> GetTaskCheck(
        Guid userId,
        Guid taskCheckId)
    {
        var result = await GetTaskToCheck(userId, taskCheckId);
        if (result.Result is not Ok<TaskCheck> ok)
            return (dynamic)result.Result;
        return TypedResults.Ok(mapper.Map<TaskCheckDto>(ok.Value));
    }

    public async Task<Results<Ok, NotFound, UnauthorizedHttpResult, ForbidHttpResult, ValidationProblem>> CheckTask(
        Guid userId, Guid taskCheckId, CheckTaskDto checkTaskDto)
    {
        var result = await GetTaskToCheck(userId, taskCheckId);
        if (result.Result is not Ok<TaskCheck> ok)
            return (dynamic)result.Result;
        var taskCheck = ok.Value!;
        await taskCheckRepository.CheckTask(taskCheck, checkTaskDto.Points);
        return TypedResults.Ok();
    }
}

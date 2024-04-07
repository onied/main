using Courses.Dtos.TaskCheckDtos;
using Courses.Dtos.TaskCheckDtos.Request;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Courses.Services;

public interface ITaskCheckService
{
    public Task<Results<Ok<TaskCheckDto>, NotFound, UnauthorizedHttpResult, ForbidHttpResult>> GetTaskCheck(Guid userId,
        Guid taskCheckId);

    public Task<Results<Ok, NotFound, UnauthorizedHttpResult, ForbidHttpResult, ValidationProblem>> CheckTask(
        Guid userId,
        Guid taskCheckId, CheckTaskDto checkTaskDto);
}

using Courses.Dtos;
using Courses.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Task = System.Threading.Tasks.Task;

namespace Courses.Services.Abstractions;

public interface ICheckTaskManagementService
{
    public Task<Results<Ok<TasksBlock>, NotFound, ForbidHttpResult>> TryGetTaskBlock(
        Guid userId, int courseId, int blockId,
        bool includeVariants = false,
        bool includeAnswers = false);

    public Results<Ok<List<UserTaskPoints>>, NotFound<string>, BadRequest<string>> GetUserTaskPoints(
        List<UserInputDto> inputsDto,
        TasksBlock block,
        Guid userId);

    public Task ManageTaskBlockCompleted(
        List<UserTaskPoints> pointsInfo,
        Guid userId,
        int blockId);
}

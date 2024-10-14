using Courses.Data.Models;
using Courses.Dtos.CheckTasks.Request;
using Task = System.Threading.Tasks.Task;

namespace Courses.Services.Abstractions;

public interface ICheckTaskManagementService
{
    public Task<IResult> TryGetTaskBlock(
        Guid userId, int courseId, int blockId,
        bool includeVariants = false,
        bool includeAnswers = false);

    public Task<IResult> GetTaskPointsStored(
        int courseId,
        int blockId,
        Guid userId,
        string? role);

    public Task<IResult> CheckTaskBlock(
        int courseId,
        int blockId,
        Guid userId,
        string? role,
        List<UserInputRequest> inputsDto);
}

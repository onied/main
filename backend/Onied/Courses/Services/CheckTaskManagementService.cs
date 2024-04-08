using Courses.Models;
using Courses.Services.Abstractions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Courses.Services;

public class CheckTaskManagementService(
    IBlockRepository blockRepository,
    IUserCourseInfoRepository userCourseInfoRepository)
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
}

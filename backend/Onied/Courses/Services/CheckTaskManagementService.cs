using Courses.Models;
using Courses.Services.Abstractions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Courses.Services;

public class CheckTaskManagementService(
    IBlockRepository blockRepository,
    IUserCourseInfoRepository userCourseInfoRepository)
    : ICheckTaskManagementService
{
    public async Task<Results<Ok<TasksBlock>, NotFound, ForbidHttpResult>> ValidateVisitPageAsync(
        Guid userId, int courseId, int blockId)
    {
        var block = await blockRepository.GetTasksBlock(blockId, true);

        if (block == null || block.Module.CourseId != courseId)
            return TypedResults.NotFound();

        if (await userCourseInfoRepository.GetUserCourseInfoAsync(userId, courseId) is null)
            return TypedResults.Forbid();

        return TypedResults.Ok(block);
    }
}

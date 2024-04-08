using Courses.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Courses.Services.Abstractions;

public interface ICheckTaskManagementService
{
    public Task<Results<Ok<TasksBlock>, NotFound, ForbidHttpResult>> TryGetTaskBlock(
        Guid userId, int courseId, int blockId,
        bool includeVariants = false,
        bool includeAnswers = false);
}

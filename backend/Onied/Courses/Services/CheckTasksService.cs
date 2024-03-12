using Courses.Models.Blocks.Tasks;
using Courses.Models.Blocks.Tasks.TaskUserInput;
using Microsoft.EntityFrameworkCore;

namespace Courses.Services;

public class CheckTasksService : ICheckTasksService
{
    private readonly IDbContextFactory<AppDbContext> _contextFactory;

    public CheckTasksService(IDbContextFactory<AppDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<UserTaskPoints?> CheckTask(UserInput input)
    {
        if (!input.IsDone)
        {
            return new UserTaskPoints()
            {
                UserId = input.UserId,
                TaskId = input.TaskId,
                Points = 0
            };
        }
        
        return await CheckTask((dynamic)input);
    }
     
    
    private async Task<UserTaskPoints> CheckTask(VariantsAnswerUserInput input)
    {
        await using var dbContext = await _contextFactory.CreateDbContextAsync();
        
        var task = await dbContext.VariantsTasks
            .Include(task => task.Variants)
            .FirstOrDefaultAsync(task => task.Id == input.TaskId);

        if (task is null)
            throw new Exception("Task not found in database");
        
        return new UserTaskPoints()
        {
            UserId = input.UserId,
            TaskId = input.TaskId,
            Points = task.Variants
                .Where(variant => variant.IsCorrect)
                .Select(variant => variant.Id)
                .SequenceEqual(input.VariantsIds) ? task.MaxPoints : 0
        };
    }
    
    private async Task<UserTaskPoints> CheckTask(InputAnswerUserInput input)
    {
        await using var dbContext = await _contextFactory.CreateDbContextAsync();
        
        var task = await dbContext.InputTasks
            .Include(task => task.Answers)
            .FirstOrDefaultAsync(task => task.Id == input.TaskId);

        if (task is null)
            throw new Exception("Task not found in database");
        
        return new UserTaskPoints()
        {
            UserId = input.UserId,
            TaskId = input.TaskId,
            Points = task.Answers.Any(
                    answer => answer.IsCaseSensitive 
                        ? answer.Answer.ToLower().Equals(input.Answer.ToLower())
                        : answer.Answer.Equals(input.Answer)
                    ) ? task.MaxPoints : 0
        };
    }
    
    private async Task<UserTaskPoints?> CheckTask(ManualReviewUserInput input)
    {
        return null;
    }
}
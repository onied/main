using Courses.Models;
using Microsoft.EntityFrameworkCore;

namespace Courses.Services;

public class BlockRepository(AppDbContext dbContext) : IBlockRepository
{
    public async Task<SummaryBlock?> GetSummaryBlock(int id)
    => await dbContext.SummaryBlocks
        .Include(block => block.Module)
        .FirstOrDefaultAsync(block => block.Id == id);

    public async Task<VideoBlock?> GetVideoBlock(int id)
        => await dbContext.VideoBlocks
            .Include(block => block.Module)
            .FirstOrDefaultAsync(block => block.Id == id);

    public Task<TasksBlock?> GetTasksBlock(
        int id,
        bool includeVariants = false,
        bool includeAnswers = false)
    {
        var query = dbContext.TasksBlocks
            .Include(block => block.Module)
            .Include(block => block.Tasks).AsQueryable();

        if (includeVariants)
        {
            query = query.Include(block => block.Tasks)
                .ThenInclude(task => ((VariantsTask)task).Variants);
        }

        if (includeAnswers)
        {
            query = query.Include(block => block.Tasks)
                .ThenInclude(task => ((InputTask)task).Answers);
        }

        return query.FirstOrDefaultAsync(block => block.Id == id);
    }

}

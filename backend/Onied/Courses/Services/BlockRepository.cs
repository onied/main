using Courses.Data;
using Courses.Data.Models;
using Courses.Services.Abstractions;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;

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

    public async Task AddBlockAsync(Block block)
    {
        block.Index = await dbContext.Blocks.Where(b => b.ModuleId == block.ModuleId).CountAsync();
        await dbContext.Blocks.AddAsync(block);
        await dbContext.SaveChangesAsync();
    }

    public async Task<int> AddBlockReturnIdAsync(Block block)
    {
        block.Index = await dbContext.Blocks.Where(b => b.ModuleId == block.ModuleId).CountAsync();
        await dbContext.Blocks.AddAsync(block);
        await dbContext.SaveChangesAsync();

        return block.Id;
    }

    public async Task UpdateSummaryBlock(SummaryBlock summaryBlock)
    {
        dbContext.SummaryBlocks.Update(summaryBlock);
        await dbContext.SaveChangesAsync();
    }

    public async Task UpdateVideoBlock(VideoBlock videoBlock)
    {
        dbContext.VideoBlocks.Update(videoBlock);
        await dbContext.SaveChangesAsync();
    }

    public async Task UpdateTasksBlock(TasksBlock tasksBlock)
    {
        dbContext.TasksBlocks.Update(tasksBlock);
        await dbContext.SaveChangesAsync();
    }

    public async Task<bool> RenameBlockAsync(int id, string title)
    {
        var block = await dbContext.Blocks.FirstOrDefaultAsync(m => m.Id == id);
        if (block != null)
        {
            block.Title = title;
            await dbContext.SaveChangesAsync();
            return true;
        }

        return false;
    }

    public async Task<bool> DeleteBlockAsync(int id)
    {
        var removedBlock = await dbContext.Blocks.FirstOrDefaultAsync(b => b.Id == id);
        if (removedBlock != null)
        {
            dbContext.Blocks.Remove(removedBlock);
            var blocks = dbContext.Blocks
                .Where(m => m.ModuleId == removedBlock.ModuleId && m.Id != id)
                .OrderBy(m => m.Index);
            var newIndex = 0;
            foreach (var block in blocks)
            {
                block.Index = newIndex++;
            }
            await dbContext.SaveChangesAsync();
            return true;
        }

        return false;
    }
}

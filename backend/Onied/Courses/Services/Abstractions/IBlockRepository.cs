using Courses.Models;
using Task = System.Threading.Tasks.Task;

namespace Courses.Services.Abstractions;

public interface IBlockRepository
{
    public Task<SummaryBlock?> GetSummaryBlock(int id);
    public Task<VideoBlock?> GetVideoBlock(int id);

    public Task<TasksBlock?> GetTasksBlock(
        int id,
        bool includeVariants = false,
        bool includeAnswers = false);

    public Task AddBlockAsync(Block block);
    public Task<int> AddBlockReturnIdAsync(Block block);

    public Task UpdateSummaryBlock(SummaryBlock summaryBlock);
    public Task UpdateVideoBlock(VideoBlock videoBlock);
    public Task UpdateTasksBlock(TasksBlock tasksBlock);
    public Task<bool> RenameBlockAsync(int id, string title);

    public Task<bool> DeleteBlockAsync(int id);
}

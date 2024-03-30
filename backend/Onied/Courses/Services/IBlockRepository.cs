using Courses.Models;

namespace Courses.Services;

public interface IBlockRepository
{
    public Task<SummaryBlock?> GetSummaryBlock(int id);
    public Task<VideoBlock?> GetVideoBlock(int id);
    public Task<TasksBlock?> GetTasksBlock(
        int id,
        bool includeVariants = false,
        bool includeAnswers = false);
}

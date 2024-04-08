using Courses.Models;
using Task = System.Threading.Tasks.Task;

namespace Courses.Services.Abstractions;

public interface IBlockCompletedInfoRepository
{
    public Task<List<BlockCompletedInfo>> GetAllCompletedCourseBlocksByUser(Guid userId, int courseId);
    public Task<BlockCompletedInfo?> GetCompletedCourseBlocksAsync(Guid userId, int blockId);
    public Task AddCompletedCourseBlocksAsync(Guid userId, int blockId);
}

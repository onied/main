using Courses.Data.Models;
using Task = System.Threading.Tasks.Task;

namespace Courses.Services.Abstractions;

public interface IBlockCompletedInfoRepository
{
    public Task<List<BlockCompletedInfo>> GetAllCompletedCourseBlocksByUser(Guid userId, int courseId);
    public Task<BlockCompletedInfo?> GetCompletedCourseBlockAsync(Guid userId, int blockId);
    public Task AddCompletedCourseBlockAsync(Guid userId, int blockId);
    public Task DeleteCompletedCourseBlocksAsync(BlockCompletedInfo blockCompletedInfo);
}

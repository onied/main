using Courses.Models;
using Courses.Services.Abstractions;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;

namespace Courses.Services;

public class BlockCompletedInfoRepository(AppDbContext dbContext) : IBlockCompletedInfoRepository
{
    public async Task<List<BlockCompletedInfo>> GetAllCompletedCourseBlocksByUser(Guid userId, int courseId)
        => await dbContext.BlockCompletedInfos
            .Include(b => b.Block)
            .ThenInclude(b => b.Module)
            .Where(b => b.UserId == userId && b.Block.Module.CourseId == courseId)
            .AsNoTracking()
            .ToListAsync();

    public async Task<BlockCompletedInfo?> GetCompletedCourseBlockAsync(Guid userId, int blockId)
        => await dbContext.BlockCompletedInfos
            .AsNoTracking()
            .SingleOrDefaultAsync(b => b.UserId == userId && b.BlockId == blockId);

    public async Task AddCompletedCourseBlockAsync(Guid userId, int blockId)
    {
        await dbContext.BlockCompletedInfos
            .AddAsync(new BlockCompletedInfo { UserId = userId, BlockId = blockId });
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteCompletedCourseBlocksAsync(BlockCompletedInfo blockCompletedInfo)
    {
        dbContext.BlockCompletedInfos.Remove(blockCompletedInfo);
        await dbContext.SaveChangesAsync();
    }
}

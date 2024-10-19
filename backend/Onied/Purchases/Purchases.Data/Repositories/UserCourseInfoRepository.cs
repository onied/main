using Microsoft.EntityFrameworkCore;
using Purchases.Data.Abstractions;
using Purchases.Data.Models;

namespace Purchases.Data.Repositories;

public class UserCourseInfoRepository(AppDbContext dbContext) : IUserCourseInfoRepository
{
    public async Task AddAsync(UserCourseInfo userCourseInfo)
    {
        await dbContext.UserCourseInfos.AddAsync(userCourseInfo);
        await dbContext.SaveChangesAsync();
    }

    public async Task<UserCourseInfo?> GetAsync(Guid userId, int courseId)
        => await dbContext.UserCourseInfos.SingleOrDefaultAsync(uci =>
            uci.UserId == userId && uci.CourseId == courseId);
}

using Courses.Models;
using Courses.Services.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Courses.Services;

public class UserCourseInfoRepository(AppDbContext dbContext) : IUserCourseInfoRepository
{
    public async Task<UserCourseInfo?> GetUserCourseInfoAsync(
        Guid userId,
        int courseId,
        bool withCourse = false)
    {
        var query = dbContext.UserCourseInfos.AsNoTracking();
        if (withCourse) query = query.Include(uci => uci.Course);

        return await query.SingleOrDefaultAsync(
            uci => uci.UserId == userId && uci.CourseId == courseId);
    }

    public async Task<bool> AddUserCourseInfoAsync(UserCourseInfo userCourseInfo)
    {
        if (await dbContext.UserCourseInfos.FindAsync(userCourseInfo.UserId, userCourseInfo.CourseId) is not null)
        {
            return false;
        }

        await dbContext.UserCourseInfos.AddAsync(userCourseInfo);
        await dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateUserCourseInfoAsync(UserCourseInfo userCourseInfo)
    {
        if (await dbContext.UserCourseInfos
                .AsQueryable()
                .AsNoTracking()
                .FirstOrDefaultAsync(uci =>
                    uci.UserId == userCourseInfo.UserId
                    && uci.CourseId == userCourseInfo.CourseId) is null) return false;

        dbContext.UserCourseInfos.Update(userCourseInfo);
        await dbContext.SaveChangesAsync();
        return true;
    }
}

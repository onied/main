using Purchases.Data.Models;

namespace Purchases.Data.Abstractions;

public interface IUserCourseInfoRepository
{
    public Task AddAsync(UserCourseInfo userCourseInfo);
    public Task<UserCourseInfo?> GetAsync(Guid userId, int courseId);
}

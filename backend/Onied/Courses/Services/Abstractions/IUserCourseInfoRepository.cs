using Courses.Models;

namespace Courses.Services.Abstractions;

public interface IUserCourseInfoRepository
{
    public Task<UserCourseInfo?> GetUserCourseInfoAsync(Guid userId, int courseId);
    public Task<bool> AddUserCourseInfoAsync(UserCourseInfo userCourseInfo);
    public Task<bool> UpdateUserCourseInfoAsync(UserCourseInfo userCourseInfo);
}

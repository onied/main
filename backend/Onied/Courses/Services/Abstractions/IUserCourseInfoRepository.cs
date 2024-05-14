using Courses.Data.Models;

namespace Courses.Services.Abstractions;

public interface IUserCourseInfoRepository
{
    public Task<UserCourseInfo?> GetUserCourseInfoAsync(Guid userId, int courseId, bool withCourse = false);
    public Task<bool> AddUserCourseInfoAsync(UserCourseInfo userCourseInfo);
    public Task<bool> UpdateUserCourseInfoAsync(UserCourseInfo userCourseInfo);
}

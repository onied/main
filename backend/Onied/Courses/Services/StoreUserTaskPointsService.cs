using Courses.Models;
using Courses.Services.Abstractions;
using Task = System.Threading.Tasks.Task;

namespace Courses.Services;

public class StoreUserTaskPointsService(IUserCourseInfoRepository userCourseInfoRepository)
    : IStoreUserTaskPointsService
{
    public async Task<List<UserTaskPoints>?> GetUserTaskPointsByCourse(Guid userId, int courseId)
    {
        var userCourseInfo = await userCourseInfoRepository
            .GetUserCourseInfoAsync(userId, courseId);
        return userCourseInfo?.UserTaskPointsStorage
            .Where(tp => tp.CourseId == courseId)
            .ToList();
    }

    public async Task<List<UserTaskPoints>?> GetUserTaskPointsByBlock(Guid userId, int courseId, int blockId)
    {
        var userCourseInfo = await userCourseInfoRepository
            .GetUserCourseInfoAsync(userId, courseId);
        return userCourseInfo?.UserTaskPointsStorage
            .Where(tp => tp.CourseId == courseId)
            .Where(tp => tp.Task.TasksBlockId == blockId)
            .ToList();
    }

    public async Task StoreUserTaskPoints(
        UserCourseInfo userCourseInfo,
        List<UserTaskPoints> userTaskPointsList)
    {
        var taskIdsToUpdate = userTaskPointsList.Select(tp => tp.TaskId).ToHashSet();
        userCourseInfo.UserTaskPointsStorage.RemoveAll(tp => taskIdsToUpdate.Contains(tp.TaskId));
        userCourseInfo.UserTaskPointsStorage.AddRange(userTaskPointsList);
        await userCourseInfoRepository.UpdateUserCourseInfoAsync(userCourseInfo);
    }
}

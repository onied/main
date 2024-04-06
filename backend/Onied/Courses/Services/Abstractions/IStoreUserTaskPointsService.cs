using Courses.Models;
using Task = System.Threading.Tasks.Task;

namespace Courses.Services.Abstractions;

public interface IStoreUserTaskPointsService
{
    public Task<List<UserTaskPoints>?> GetUserTaskPointsByCourse(Guid userId, int courseId);
    public Task<List<UserTaskPoints>?> GetUserTaskPointsByBlock(Guid userId, int courseId, int blockId);
    public Task StoreUserTaskPoints(
        UserCourseInfo userCourseInfo,
        List<UserTaskPoints> userTaskPointsList);
}

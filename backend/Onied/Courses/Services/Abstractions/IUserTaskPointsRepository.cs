using Courses.Models;
using Task = System.Threading.Tasks.Task;

namespace Courses.Services.Abstractions;

public interface IUserTaskPointsRepository
{
    public Task<List<UserTaskPoints>> GetUserTaskPointsByUserAndCourse(Guid userId, int courseId);
    public Task<List<UserTaskPoints>> GetUserTaskPointsByUserAndBlock(Guid userId, int courseId, int blockId);

    public Task StoreUserTaskPointsForConcreteUserAndBlock(
        List<UserTaskPoints> userTaskPointsList,
        Guid userId,
        int courseId,
        int blockId);
}

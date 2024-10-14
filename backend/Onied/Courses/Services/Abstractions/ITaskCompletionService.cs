using Courses.Data.Models;
using Courses.Dtos.CheckTasks.Request;
using Task = System.Threading.Tasks.Task;

namespace Courses.Services.Abstractions;

public interface ITaskCompletionService
{
    public IResult GetUserTaskPoints(
        List<UserInputRequest> inputsDto,
        TasksBlock block,
        Guid userId);

    public Task ManageTaskBlockCompleted(
        List<UserTaskPoints> pointsInfo,
        Guid userId,
        int blockId);

    public Task ManageCourseCompleted(Guid userId, int courseId);
}

using Courses.Models.Blocks.Tasks;
using Courses.Models.Blocks.Tasks.TaskUserInput;

namespace Courses.Services;

public interface ICheckTasksService
{
    public Task<UserTaskPoints?> CheckTask(UserInput input);
}
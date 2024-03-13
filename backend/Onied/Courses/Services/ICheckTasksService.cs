using Courses.Dtos.Blocks.Tasks;
using Courses.Models.Blocks.Tasks;
using Task = Courses.Models.Task;

namespace Courses.Services;

public interface ICheckTasksService
{
    public UserTaskPoints? CheckTask(Task task, UserInputDto input);
}
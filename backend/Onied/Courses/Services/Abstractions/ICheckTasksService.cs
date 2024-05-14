using Courses.Data.Models;
using Courses.Dtos.CheckTasks.Request;
using Task = Courses.Data.Models.Task;

namespace Courses.Services.Abstractions;

public interface ICheckTasksService
{
    public UserTaskPoints CheckTask(Task task, UserInputRequest input);
}

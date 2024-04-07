using Courses.Models;
using Task = System.Threading.Tasks.Task;

namespace Courses.Services;

public interface ITaskCheckRepository
{
    public Task<List<TaskCheck>> GetTasksToCheck(User teacher);
    public Task<List<TaskCheck>> GetUncheckedTasks(User teacher);
    public Task<List<TaskCheck>> GetCheckedTasks(User teacher);
    public Task<TaskCheck?> GetTaskCheck(Guid taskCheckId);
    public bool CanCheckTask(User teacher, TaskCheck taskCheck);
    public Task CheckTask(TaskCheck taskCheck, int points);
}
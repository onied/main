using Courses.Dtos;
using Courses.Models;

namespace Courses.Services;

public interface IUpdateTasksBlockService
{
    public Task<TasksBlock> UpdateTasksBlock(EditTasksBlockDto tasksBlockDto);
}

using Courses.Dtos;
using Courses.Models;

namespace Courses.Services.Abstractions;

public interface IUpdateTasksBlockService
{
    public Task<TasksBlock> UpdateTasksBlock(EditTasksBlockDto tasksBlockDto);
}

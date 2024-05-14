using Courses.Dtos;
using Courses.Dtos.EditCourse.Request;
using Courses.Models;

namespace Courses.Services.Abstractions;

public interface IUpdateTasksBlockService
{
    public Task<TasksBlock> UpdateTasksBlock(EditTasksBlockRequest tasksBlockRequest);
}

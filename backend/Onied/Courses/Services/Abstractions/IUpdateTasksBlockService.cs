using Courses.Data.Models;
using Courses.Dtos.EditCourse.Request;

namespace Courses.Services.Abstractions;

public interface IUpdateTasksBlockService
{
    public Task<TasksBlock> UpdateTasksBlock(EditTasksBlockRequest tasksBlockRequest);
}

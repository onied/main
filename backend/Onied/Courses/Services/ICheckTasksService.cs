using Courses.Dtos;
using Courses.Models;
using Task = Courses.Models.Task;

namespace Courses.Services;

public interface ICheckTasksService
{
    public UserTaskPoints? CheckTask(Task task, UserInputDto input);
}
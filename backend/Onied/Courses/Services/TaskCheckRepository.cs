using Courses.Models;
using Microsoft.EntityFrameworkCore;

namespace Courses.Services;

public class TaskCheckRepository(AppDbContext context) : ITaskCheckRepository
{
    public async Task<List<TaskCheck>> GetTasksToCheck(User teacher)
    {
        return (await context.TaskChecks
                .AsNoTracking()
                .Include(check => check.Task)
                .ThenInclude(task => task.TasksBlock)
                .ThenInclude(block => block.Module)
                .ThenInclude(module => module.Course)
                .ToListAsync())
            .Where(check => CanCheckTask(teacher, check)).ToList();
    }

    public async Task<List<TaskCheck>> GetUncheckedTasks(User teacher)
    {
        return (await GetTasksToCheck(teacher)).Where(check => !check.Checked).ToList();
    }

    public async Task<List<TaskCheck>> GetCheckedTasks(User teacher)
    {
        return (await GetTasksToCheck(teacher)).Where(check => check.Checked).ToList();
    }

    public Task<TaskCheck?> GetTaskCheck(Guid taskCheckId)
    {
        return context.TaskChecks
            .AsNoTracking()
            .Include(check => check.Task)
            .ThenInclude(task => task.TasksBlock)
            .ThenInclude(block => block.Module)
            .ThenInclude(module => module.Course)
            .FirstOrDefaultAsync(check => check.Id == taskCheckId);
    }

    public bool CanCheckTask(User teacher, TaskCheck taskCheck)
    {
        var targetCourse = taskCheck.Task.TasksBlock.Module.Course;
        var teachingCourse = teacher.TeachingCourses.SingleOrDefault(course => course.Id == targetCourse.Id);
        var moderatingCourse = teacher.ModeratingCourses.SingleOrDefault(course => course.Id == targetCourse.Id);
        return teachingCourse != null || moderatingCourse != null;
    }
}

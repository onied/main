using Courses.Models;
using Microsoft.EntityFrameworkCore;

namespace Courses.Services;

public class ManualReviewTaskUserAnswerRepository(AppDbContext context) : IManualReviewTaskUserAnswerRepository
{
    public async Task<List<ManualReviewTaskUserAnswer>> GetTasksToReview(User teacher)
    {
        return (await context.ManualReviewTaskUserAnswers
                .AsNoTracking()
                .Include(check => check.Task)
                .ThenInclude(task => task.TasksBlock)
                .ThenInclude(block => block.Module)
                .ThenInclude(module => module.Course)
                .ToListAsync())
            .Where(check => CanReviewAnswer(teacher, check)).ToList();
    }

    public Task<ManualReviewTaskUserAnswer?> GetById(Guid taskCheckId)
    {
        return context.ManualReviewTaskUserAnswers
            .AsNoTracking()
            .Include(check => check.Task)
            .ThenInclude(task => task.TasksBlock)
            .ThenInclude(block => block.Module)
            .ThenInclude(module => module.Course)
            .FirstOrDefaultAsync(check => check.Id == taskCheckId);
    }

    public bool CanReviewAnswer(User teacher, ManualReviewTaskUserAnswer answer)
    {
        var targetCourse = answer.Task.TasksBlock.Module.Course;
        var teachingCourse = teacher.TeachingCourses.SingleOrDefault(course => course.Id == targetCourse.Id);
        var moderatingCourse = teacher.ModeratingCourses.SingleOrDefault(course => course.Id == targetCourse.Id);
        return teachingCourse != null || moderatingCourse != null;
    }
}

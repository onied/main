using Courses.Data;
using Courses.Data.Models;
using Courses.Dtos.ManualReview.Request;
using Courses.Services.Abstractions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Courses.Services;

public class ManualReviewTaskUserAnswerRepository(AppDbContext context) : IManualReviewTaskUserAnswerRepository
{
    public async Task<List<ManualReviewTaskUserAnswer>> GetTasksToReview(User teacher)
    {
        return (await context.ManualReviewTaskUserAnswers
                .AsNoTracking()
                .Include(answer => answer.Task)
                .ThenInclude(task => task.TasksBlock)
                .ThenInclude(block => block.Module)
                .ThenInclude(module => module.Course)
                .ToListAsync())
            .Where(answer => CanReviewAnswer(teacher, answer)).ToList();
    }

    public async Task<List<ManualReviewTaskUserAnswer>> GetUncheckedTasksToReview(User teacher)
    {
        return (await context.ManualReviewTaskUserAnswers
                .AsNoTracking()
                .Include(answer => answer.Task)
                .ThenInclude(task => task.TasksBlock)
                .ThenInclude(block => block.Module)
                .ThenInclude(module => module.Course)
                .Where(answer => !answer.Checked)
                .ToListAsync())
            .Where(answer => CanReviewAnswer(teacher, answer)).ToList();
    }

    public async Task<List<ManualReviewTaskUserAnswer>> GetCheckedTasksToReview(User teacher)
    {
        return (await context.ManualReviewTaskUserAnswers
                .AsNoTracking()
                .Include(answer => answer.Task)
                .ThenInclude(task => task.TasksBlock)
                .ThenInclude(block => block.Module)
                .ThenInclude(module => module.Course)
                .Where(answer => answer.Checked)
                .ToListAsync())
            .Where(answer => CanReviewAnswer(teacher, answer)).ToList();
    }

    public async Task<ManualReviewTaskUserAnswer?> GetById(Guid userAnswerId)
    {
        return await context.ManualReviewTaskUserAnswers
            .Include(check => check.Task)
            .ThenInclude(task => task.TasksBlock)
            .ThenInclude(block => block.Module)
            .ThenInclude(module => module.Course)
            .FirstOrDefaultAsync(answer => answer.ManualReviewTaskUserAnswerId == userAnswerId);
    }

    public bool CanReviewAnswer(User teacher, ManualReviewTaskUserAnswer answer)
    {
        var targetCourse = answer.Task.TasksBlock.Module.Course;
        var teachingCourse = teacher.TeachingCourses.SingleOrDefault(course => course.Id == targetCourse.Id);
        var moderatingCourse = teacher.ModeratingCourses.SingleOrDefault(course => course.Id == targetCourse.Id);
        return teachingCourse != null || moderatingCourse != null;
    }

    public async Task<ValidationProblem?> ReviewAnswer(ReviewTaskRequest reviewTaskRequest, ManualReviewTaskUserAnswer answer)
    {
        if (reviewTaskRequest.Points > answer.Task.MaxPoints)
            return TypedResults.ValidationProblem(new Dictionary<string, string[]>
            {
                [nameof(reviewTaskRequest.Points)] = ["Points cannot exceed task max points"]
            });
        if (reviewTaskRequest.Points < 0)
            return TypedResults.ValidationProblem(new Dictionary<string, string[]>
            {
                [nameof(reviewTaskRequest.Points)] = ["Points cannot be negative"]
            });
        answer.Checked = true;
        answer.Points = reviewTaskRequest.Points;
        context.Update(answer);
        await context.SaveChangesAsync();
        return null;
    }
}

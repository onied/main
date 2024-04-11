using Courses.Dtos.ManualReviewDtos.Request;
using Courses.Models;
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

    public Task<ManualReviewTaskUserAnswer?> GetById(Guid userAnswerId)
    {
        return context.ManualReviewTaskUserAnswers
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

    public async Task<ValidationProblem?> ReviewAnswer(ReviewTaskDto reviewTaskDto, ManualReviewTaskUserAnswer answer)
    {
        if (reviewTaskDto.Points > answer.Task.MaxPoints)
            return TypedResults.ValidationProblem(new Dictionary<string, string[]>
            {
                [nameof(reviewTaskDto.Points)] = ["Points cannot exceed task max points"]
            });
        if (reviewTaskDto.Points < 0)
            return TypedResults.ValidationProblem(new Dictionary<string, string[]>
            {
                [nameof(reviewTaskDto.Points)] = ["Points cannot be negative"]
            });
        answer.Checked = true;
        answer.Points = reviewTaskDto.Points;
        context.Update(answer);
        await context.SaveChangesAsync();
        return null;
    }
}

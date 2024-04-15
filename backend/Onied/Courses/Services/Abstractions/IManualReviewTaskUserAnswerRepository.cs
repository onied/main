using Courses.Dtos.ManualReviewDtos.Request;
using Courses.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Courses.Services.Abstractions;

public interface IManualReviewTaskUserAnswerRepository
{
    public Task<List<ManualReviewTaskUserAnswer>> GetTasksToReview(User teacher);
    public Task<List<ManualReviewTaskUserAnswer>> GetUncheckedTasksToReview(User teacher);
    public Task<List<ManualReviewTaskUserAnswer>> GetCheckedTasksToReview(User teacher);
    public Task<ManualReviewTaskUserAnswer?> GetById(Guid userAnswerId);
    public bool CanReviewAnswer(User teacher, ManualReviewTaskUserAnswer answer);
    public Task<ValidationProblem?> ReviewAnswer(ReviewTaskDto reviewTaskDto, ManualReviewTaskUserAnswer answer);
}

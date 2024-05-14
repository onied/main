using Courses.Data.Models;
using Courses.Dtos.ManualReview.Request;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Courses.Services.Abstractions;

public interface IManualReviewTaskUserAnswerRepository
{
    public Task<List<ManualReviewTaskUserAnswer>> GetTasksToReview(User teacher);
    public Task<List<ManualReviewTaskUserAnswer>> GetUncheckedTasksToReview(User teacher);
    public Task<List<ManualReviewTaskUserAnswer>> GetCheckedTasksToReview(User teacher);
    public Task<ManualReviewTaskUserAnswer?> GetById(Guid userAnswerId);
    public bool CanReviewAnswer(User teacher, ManualReviewTaskUserAnswer answer);
    public Task<ValidationProblem?> ReviewAnswer(ReviewTaskRequest reviewTaskRequest, ManualReviewTaskUserAnswer answer);
}

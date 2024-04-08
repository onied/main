using Courses.Models;

namespace Courses.Services;

public interface IManualReviewTaskUserAnswerRepository
{
    public Task<List<ManualReviewTaskUserAnswer>> GetTasksToReview(User teacher);
    public Task<ManualReviewTaskUserAnswer?> GetById(Guid taskCheckId);
    public bool CanReviewAnswer(User teacher, ManualReviewTaskUserAnswer answer);
}

using Courses.Dtos.ManualReview.Request;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Courses.Services.Abstractions;

public interface IManualReviewService
{
    public Task<IResult>
        GetManualReviewTaskUserAnswer(Guid userId,
            Guid manualReviewTaskUserAnswerId);

    public Task<IResult> ReviewUserAnswer(
        Guid userId,
        Guid manualReviewTaskUserAnswerId, ReviewTaskRequest reviewTaskRequest);

    public Task<IResult> GetUncheckedForTeacher(
        Guid teacherId);

    public Task<IResult> GetCheckedForTeacher(
        Guid teacherId);

    public Task<IResult> GetTasksToCheckForTeacher(
        Guid teacherId);
}

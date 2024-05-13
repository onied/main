using Courses.Dtos.ManualReviewDtos.Request;
using Courses.Dtos.ManualReviewDtos.Response;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Courses.Services.Abstractions;

public interface IManualReviewService
{
    public Task<IResult>
        GetManualReviewTaskUserAnswer(Guid userId,
            Guid manualReviewTaskUserAnswerId);

    public Task<IResult> ReviewUserAnswer(
        Guid userId,
        Guid manualReviewTaskUserAnswerId, ReviewTaskDto reviewTaskDto);

    public Task<IResult> GetUncheckedForTeacher(
        Guid teacherId);

    public Task<IResult> GetCheckedForTeacher(
        Guid teacherId);

    public Task<IResult> GetTasksToCheckForTeacher(
        Guid teacherId);
}

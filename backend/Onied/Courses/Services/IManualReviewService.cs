using Courses.Dtos.ManualReviewDtos.Request;
using Courses.Dtos.ManualReviewDtos.Response;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Courses.Services;

public interface IManualReviewService
{
    public Task<Results<Ok<ManualReviewTaskUserAnswerDto>, NotFound, UnauthorizedHttpResult, ForbidHttpResult>>
        GetManualReviewTaskUserAnswer(Guid userId,
            Guid manualReviewTaskUserAnswerId);

    public Task<Results<Ok, NotFound, UnauthorizedHttpResult, ForbidHttpResult, ValidationProblem>> ReviewUserAnswer(
        Guid userId,
        Guid manualReviewTaskUserAnswerId, ReviewTaskDto reviewTaskDto);

    public Task<Results<Ok<List<ManualReviewTaskUserAnswerDto>>, UnauthorizedHttpResult>> GetUncheckedForTeacher(
        Guid teacherId);

    public Task<Results<Ok<List<ManualReviewTaskUserAnswerDto>>, UnauthorizedHttpResult>> GetCheckedForTeacher(
        Guid teacherId);
}

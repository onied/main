using AutoMapper;
using Courses.Dtos.ManualReviewDtos.Request;
using Courses.Dtos.ManualReviewDtos.Response;
using Courses.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Courses.Services;

public class ManualReviewService(
    IUserRepository userRepository,
    IManualReviewTaskUserAnswerRepository manualReviewTaskUserAnswerRepository,
    IMapper mapper) : IManualReviewService
{
    private async Task<Results<Ok<ManualReviewTaskUserAnswer>, NotFound, UnauthorizedHttpResult,
            ForbidHttpResult>>
        GetByTeacherAndId(
            Guid teacherId,
            Guid manualReviewTaskUserAnswerId)
    {
        var user = await userRepository.GetUserWithModeratingAndTeachingCoursesAsync(teacherId);
        if (user == null)
            return TypedResults.Unauthorized();
        var taskCheck = await manualReviewTaskUserAnswerRepository.GetById(manualReviewTaskUserAnswerId);
        if (taskCheck == null)
            return TypedResults.NotFound();
        if (!manualReviewTaskUserAnswerRepository.CanReviewAnswer(user, taskCheck))
            return TypedResults.Forbid();
        return TypedResults.Ok(taskCheck);
    }

    public async Task<Results<Ok<ManualReviewTaskUserAnswerDto>, NotFound, UnauthorizedHttpResult, ForbidHttpResult>>
        GetManualReviewTaskUserAnswer(Guid userId, Guid manualReviewTaskUserAnswerId)
    {
        var result = await GetByTeacherAndId(userId, manualReviewTaskUserAnswerId);
        if (result.Result is not Ok<ManualReviewTaskUserAnswerRepository> ok)
            return (dynamic)result.Result;
        // TODO: Set checked and points
        return TypedResults.Ok(mapper.Map<ManualReviewTaskUserAnswerDto>(ok.Value));
    }

    public async Task<Results<Ok, NotFound, UnauthorizedHttpResult, ForbidHttpResult, ValidationProblem>>
        ReviewUserAnswer(Guid userId, Guid manualReviewTaskUserAnswerId, ReviewTaskDto reviewTaskDto)
    {
        var result = await GetByTeacherAndId(userId, manualReviewTaskUserAnswerId);
        if (result.Result is not Ok<ManualReviewTaskUserAnswer> ok)
            return (dynamic)result.Result;
        var taskCheck = ok.Value!;
        // await manualReviewTaskUserAnswerRepository.CheckTask(taskCheck.Id, reviewTaskDto.Points);
        // TODO: Add userTaskPointsRepository
        return TypedResults.Ok();
    }

    public async Task<Results<Ok<List<ManualReviewTaskUserAnswerDto>>, UnauthorizedHttpResult>> GetUncheckedForTeacher(Guid teacherId)
    {
        var user = await userRepository.GetUserWithModeratingAndTeachingCoursesAsync(teacherId);
        if (user == null)
            return TypedResults.Unauthorized();
        var result = await manualReviewTaskUserAnswerRepository.GetTasksToReview(user);

    }

    public async Task<Results<Ok<List<ManualReviewTaskUserAnswerDto>>, UnauthorizedHttpResult>> GetCheckedForTeacher(Guid teacherId)
    {
        throw new NotImplementedException();
    }
}

using AutoMapper;
using Courses.Dtos.ManualReviewDtos.Request;
using Courses.Dtos.ManualReviewDtos.Response;
using Courses.Models;
using Courses.Services.Abstractions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Courses.Services;

public class ManualReviewService(
    IUserRepository userRepository,
    IManualReviewTaskUserAnswerRepository manualReviewTaskUserAnswerRepository,
    ICheckTaskManagementService checkTaskManagementService,
    IUserTaskPointsRepository userTaskPointsRepository,
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
        if (result.Result is not Ok<ManualReviewTaskUserAnswer> ok)
            return (dynamic)result.Result;
        return TypedResults.Ok(mapper.Map<ManualReviewTaskUserAnswerDto>(ok.Value));
    }

    public async Task<Results<Ok, NotFound, UnauthorizedHttpResult, ForbidHttpResult, ValidationProblem>>
        ReviewUserAnswer(Guid userId, Guid manualReviewTaskUserAnswerId, ReviewTaskDto reviewTaskDto)
    {
        var result = await GetByTeacherAndId(userId, manualReviewTaskUserAnswerId);
        if (result.Result is not Ok<ManualReviewTaskUserAnswer> ok)
            return (dynamic)result.Result;
        var answer = ok.Value!;
        var problem = await manualReviewTaskUserAnswerRepository.ReviewAnswer(reviewTaskDto, answer);
        if (problem != null)
            return problem;
        var pointsInfo = (await userTaskPointsRepository
                .GetUserTaskPointsByUserAndBlock(userId, answer.Task.TasksBlock.Module.CourseId,
                    answer.Task.TasksBlockId))
            .ToList();
        await checkTaskManagementService.ManageTaskBlockCompleted(pointsInfo, answer.UserId, answer.Task.TasksBlockId);
        await checkTaskManagementService.ManageCourseCompleted(answer.UserId, answer.CourseId);
        return TypedResults.Ok();
    }

    public async Task<Results<Ok<List<ManualReviewTaskUserAnswerDto>>, UnauthorizedHttpResult>> GetUncheckedForTeacher(Guid teacherId)
    {
        var user = await userRepository.GetUserWithModeratingAndTeachingCoursesAsync(teacherId);
        if (user == null)
            return TypedResults.Unauthorized();
        var result = await manualReviewTaskUserAnswerRepository.GetUncheckedTasksToReview(user);
        return TypedResults.Ok(mapper.Map<List<ManualReviewTaskUserAnswerDto>>(result));
    }

    public async Task<Results<Ok<List<ManualReviewTaskUserAnswerDto>>, UnauthorizedHttpResult>> GetCheckedForTeacher(Guid teacherId)
    {
        var user = await userRepository.GetUserWithModeratingAndTeachingCoursesAsync(teacherId);
        if (user == null)
            return TypedResults.Unauthorized();
        var result = await manualReviewTaskUserAnswerRepository.GetCheckedTasksToReview(user);
        return TypedResults.Ok(mapper.Map<List<ManualReviewTaskUserAnswerDto>>(result));
    }

    public async Task<Results<Ok<List<CourseWithManualReviewTasksDto>>, UnauthorizedHttpResult>> GetTasksToCheckForTeacher(Guid teacherId)
    {
        var user = await userRepository.GetUserWithModeratingAndTeachingCoursesAsync(teacherId);
        if (user == null)
            return TypedResults.Unauthorized();
        var result = await manualReviewTaskUserAnswerRepository.GetUncheckedTasksToReview(user);
        return TypedResults.Ok(mapper.Map<List<CourseWithManualReviewTasksDto>>(result));
    }
}

using AutoMapper;
using Courses.Data.Models;
using Courses.Dtos.ManualReview.Request;
using Courses.Dtos.ManualReview.Response;
using Courses.Services.Abstractions;
using Courses.Services.Producers.NotificationSentProducer;
using MassTransit.Data.Messages;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Courses.Services;

public class ManualReviewService(
    IUserRepository userRepository,
    IManualReviewTaskUserAnswerRepository manualReviewTaskUserAnswerRepository,
    ICheckTaskManagementService checkTaskManagementService,
    IUserTaskPointsRepository userTaskPointsRepository,
    INotificationSentProducer notificationSentProducer,
    IMapper mapper) : IManualReviewService
{
    private async Task<IResult>
        GetByTeacherAndId(
            Guid teacherId,
            Guid manualReviewTaskUserAnswerId)
    {
        var user = await userRepository.GetUserWithModeratingAndTeachingCoursesAsync(teacherId);
        if (user == null)
            return Results.Unauthorized();

        var taskCheck = await manualReviewTaskUserAnswerRepository.GetById(manualReviewTaskUserAnswerId);

        if (taskCheck == null)
            return Results.NotFound();

        if (!manualReviewTaskUserAnswerRepository.CanReviewAnswer(user, taskCheck))
            return Results.Forbid();

        return Results.Ok(taskCheck);
    }

    public async Task<IResult>
        GetManualReviewTaskUserAnswer(Guid userId, Guid manualReviewTaskUserAnswerId)
    {
        var result = await GetByTeacherAndId(userId, manualReviewTaskUserAnswerId);
        if (result is not Ok<ManualReviewTaskUserAnswer> ok)
            return result;

        return Results.Ok(mapper.Map<ManualReviewTaskUserAnswerResponse>(ok.Value));
    }

    public async Task<IResult>
        ReviewUserAnswer(Guid userId, Guid manualReviewTaskUserAnswerId, ReviewTaskRequest reviewTaskRequest)
    {
        var result = await GetByTeacherAndId(userId, manualReviewTaskUserAnswerId);
        if (result is not Ok<ManualReviewTaskUserAnswer> ok)
            return result;
        var answer = ok.Value!;
        var problem = await manualReviewTaskUserAnswerRepository.ReviewAnswer(reviewTaskRequest, answer);
        if (problem != null)
            return problem;
        var pointsInfo = (await userTaskPointsRepository
                .GetUserTaskPointsByUserAndBlock(userId, answer.Task.TasksBlock.Module.CourseId,
                    answer.Task.TasksBlockId))
            .ToList();

        var course = answer.Task.TasksBlock.Module.Course;
        var notificationSent = new NotificationSent(
            course.Title,
            $"Задание \"{answer.Task.Title}\" проверено!",
            userId,
            course.PictureHref);
        await notificationSentProducer.PublishForOne(notificationSent);

        await checkTaskManagementService.ManageTaskBlockCompleted(pointsInfo, answer.UserId, answer.Task.TasksBlockId);
        await checkTaskManagementService.ManageCourseCompleted(answer.UserId, answer.CourseId);
        return Results.Ok();
    }

    public async Task<IResult> GetUncheckedForTeacher(Guid teacherId)
    {
        var user = await userRepository.GetUserWithModeratingAndTeachingCoursesAsync(teacherId);
        if (user == null)
            return Results.Unauthorized();
        var result = await manualReviewTaskUserAnswerRepository.GetUncheckedTasksToReview(user);
        return Results.Ok(mapper.Map<List<ManualReviewTaskUserAnswerResponse>>(result));
    }

    public async Task<IResult> GetCheckedForTeacher(Guid teacherId)
    {
        var user = await userRepository.GetUserWithModeratingAndTeachingCoursesAsync(teacherId);
        if (user == null)
            return Results.Unauthorized();
        var result = await manualReviewTaskUserAnswerRepository.GetCheckedTasksToReview(user);
        return Results.Ok(mapper.Map<List<ManualReviewTaskUserAnswerResponse>>(result));
    }

    public async Task<IResult> GetTasksToCheckForTeacher(Guid teacherId)
    {
        var user = await userRepository.GetUserWithModeratingAndTeachingCoursesAsync(teacherId);
        if (user == null)
            return Results.Unauthorized();
        var result = await manualReviewTaskUserAnswerRepository.GetUncheckedTasksToReview(user);
        return Results.Ok(mapper.Map<List<CourseWithManualReviewTasksResponse>>(result));
    }
}

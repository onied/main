using AutoMapper;
using Courses.Data.Models;
using Courses.Dtos.CheckTasks.Request;
using Courses.Dtos.CheckTasks.Response;
using Courses.Services.Abstractions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Courses.Services;

public class CheckTaskManagementService(
    IBlockRepository blockRepository,
    IUserCourseInfoRepository userCourseInfoRepository,
    ITaskCompletionService taskCompletionService,
    ICourseManagementService courseManagementService,
    IUserTaskPointsRepository userTaskPointsRepository,
    IMapper mapper)
    : ICheckTaskManagementService
{
    public async Task<IResult> TryGetTaskBlock(
        Guid userId, int courseId, int blockId,
        bool includeVariants = false,
        bool includeAnswers = false)
    {
        var block = await blockRepository.GetTasksBlock(blockId, includeVariants, includeAnswers);

        if (block == null || block.Module.CourseId != courseId)
            return Results.NotFound();

        if (await userCourseInfoRepository.GetUserCourseInfoAsync(userId, courseId) is null)
            return Results.Forbid();

        return Results.Ok(block);
    }

    public async Task<IResult> GetTaskPointsStored(
        int courseId,
        int blockId,
        Guid userId,
        string? role)
    {
        if (!await courseManagementService.AllowVisitCourse(userId, courseId))
            return Results.Forbid();

        var response = await TryGetTaskBlock(userId, courseId, blockId, true);
        if (response is not Ok<TasksBlock> ok)
            return response;
        var block = ok.Value!;

        var storedPoints = (await userTaskPointsRepository
            .GetUserTaskPointsByUserAndBlock(userId, courseId, blockId)).ToList();

        var points = block.Tasks.Select(task =>
            {
                var points = storedPoints.SingleOrDefault(tp => tp.TaskId == task.Id);
                if (points == null)
                    return new UserTaskPointsResponse
                    {
                        TaskId = task.Id,
                        Points = null
                    };
                var dto = mapper.Map<UserTaskPointsResponse>(points);
                if (!points.Checked)
                    dto.Points = null;
                if (points is ManualReviewTaskUserAnswer manualReviewTaskUserAnswer)
                    dto.Content = manualReviewTaskUserAnswer.Content;
                return dto;
            }
        ).ToList();

        return Results.Ok(points);
    }

    public async Task<IResult> CheckTaskBlock(
            int courseId,
            int blockId,
            Guid userId,
            string? role,
            List<UserInputRequest> inputsDto)
    {
        if (!await courseManagementService.AllowVisitCourse(userId, courseId))
            return Results.Forbid();

        var responseGetTaskBlock = await TryGetTaskBlock(userId, courseId, blockId, true, true);
        if (responseGetTaskBlock is not Ok<TasksBlock> okGetTaskBlock)
            return responseGetTaskBlock;

        var block = okGetTaskBlock.Value!;

        var responseGetTaskPoints = taskCompletionService.GetUserTaskPoints(inputsDto, block, userId);
        if (responseGetTaskPoints is not Ok<List<UserTaskPoints>> okGetTaskPoints)
            return responseGetTaskPoints;

        var pointsInfo = okGetTaskPoints.Value!;

        await userTaskPointsRepository
            .StoreUserTaskPointsForConcreteUserAndBlock(
                pointsInfo, userId, courseId, blockId);
        await taskCompletionService.ManageTaskBlockCompleted(pointsInfo, userId, blockId);
        await taskCompletionService.ManageCourseCompleted(userId, courseId);

        var pointsPrepared = block.Tasks
            .Select(task =>
            {
                var points = pointsInfo.SingleOrDefault(tp => tp.TaskId == task.Id);
                if (points == null)
                    return new UserTaskPointsResponse
                    {
                        TaskId = task.Id,
                        Points = null
                    };
                var dto = mapper.Map<UserTaskPointsResponse>(points);
                if (!points.Checked)
                    dto.Points = null;
                if (points is ManualReviewTaskUserAnswer manualReviewTaskUserAnswer)
                    dto.Content = manualReviewTaskUserAnswer.Content;
                return dto;
            }).ToList();

        return Results.Ok(pointsPrepared);
    }
}

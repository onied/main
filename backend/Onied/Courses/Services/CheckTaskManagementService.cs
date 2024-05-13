using AutoMapper;
using Courses.Dtos;
using Courses.Models;
using Courses.Services.Abstractions;
using Courses.Services.Producers.CourseCompletedProducer;
using Courses.Services.Producers.NotificationSentProducer;
using MassTransit.Data.Messages;
using Microsoft.AspNetCore.Http.HttpResults;
using Task = System.Threading.Tasks.Task;

namespace Courses.Services;

public class CheckTaskManagementService(
    ICourseRepository courseRepository,
    IBlockRepository blockRepository,
    IBlockCompletedInfoRepository blockCompletedInfoRepository,
    IUserCourseInfoRepository userCourseInfoRepository,
    ICheckTasksService checkTasksService,
    ICourseCompletedProducer courseCompletedProducer,
    ICourseManagementService courseManagementService,
    IUserTaskPointsRepository userTaskPointsRepository,
    INotificationSentProducer notificationSentProducer,
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

    public IResult GetUserTaskPoints(
        List<UserInputDto> inputsDto,
        TasksBlock block,
        Guid userId)
    {
        var points = new List<UserTaskPoints>();
        foreach (var inputDto in inputsDto)
        {
            var task = block.Tasks.SingleOrDefault(task => inputDto.TaskId == task.Id);

            if (task is null)
                return Results.NotFound($"Task with id={inputDto.TaskId} not found.");

            if (task.TaskType != inputDto.TaskType)
                return Results.BadRequest(
                    $"Task with id={inputDto.TaskId} has invalid TaskType={inputDto.TaskType}.");

            var tp = checkTasksService.CheckTask(task, inputDto);
            tp.UserId = userId;
            tp.CourseId = block.Module.CourseId;
            points.Add(tp);
        }

        return Results.Ok(points);
    }

    public async Task ManageTaskBlockCompleted(List<UserTaskPoints> pointsInfo, Guid userId, int blockId)
    {
        var bci = await blockCompletedInfoRepository
            .GetCompletedCourseBlockAsync(userId, blockId);
        if (pointsInfo.All(utp => utp.HasFullPoints) && bci is null)
        {
            await blockCompletedInfoRepository.AddCompletedCourseBlockAsync(userId, blockId);
        }
        else if (pointsInfo.Any(utp => !utp.HasFullPoints)
                 && bci is not null)
        {
            await blockCompletedInfoRepository.DeleteCompletedCourseBlocksAsync(bci);
        }
    }

    public async Task ManageCourseCompleted(Guid userId, int courseId)
    {
        var course = (await courseRepository.GetCourseWithBlocksAsync(courseId))!;
        var courseBlocks = course.Modules
            .SelectMany(m => m.Blocks)
            .Where(b => b.BlockType is BlockType.TasksBlock)
            .Select(b => b.Id).Order();
        var userBlocks = (await blockCompletedInfoRepository
            .GetAllCompletedCourseBlocksByUser(userId, courseId))
            .Where(b => b.Block.BlockType is BlockType.TasksBlock)
            .Select(b => b.BlockId).Order();

        if (courseBlocks.SequenceEqual(userBlocks))
        {
            await courseCompletedProducer.PublishAsync(new CourseCompleted(userId, courseId));

            var notificationSent = new NotificationSent(
                course.Title,
                "Вы успешно закончили обучение на курсе!",
                userId,
                course.PictureHref);
            await notificationSentProducer.PublishForOne(notificationSent);
        }
    }

    public async Task<IResult> GetTaskPointsStored(
        int courseId,
        int blockId,
        Guid userId)
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
                    return new UserTaskPointsDto
                    {
                        TaskId = task.Id,
                        Points = null
                    };
                var dto = mapper.Map<UserTaskPointsDto>(points);
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
            List<UserInputDto> inputsDto)
    {
        if (!await courseManagementService.AllowVisitCourse(userId, courseId))
            return TypedResults.Forbid();

        var responseGetTaskBlock = await TryGetTaskBlock(userId, courseId, blockId, true, true);
        if (responseGetTaskBlock is not Ok<TasksBlock> okGetTaskBlock)
            return responseGetTaskBlock;

        var block = okGetTaskBlock.Value!;

        var responseGetTaskPoints = GetUserTaskPoints(inputsDto, block, userId);
        if (responseGetTaskPoints is not Ok<List<UserTaskPoints>> okGetTaskPoints)
            return responseGetTaskPoints;

        var pointsInfo = okGetTaskPoints.Value!;

        await userTaskPointsRepository
            .StoreUserTaskPointsForConcreteUserAndBlock(
                pointsInfo, userId, courseId, blockId);
        await ManageTaskBlockCompleted(pointsInfo, userId, blockId);
        await ManageCourseCompleted(userId, courseId);

        var pointsPrepared = block.Tasks
            .Select(task =>
            {
                var points = pointsInfo.SingleOrDefault(tp => tp.TaskId == task.Id);
                if (points == null)
                    return new UserTaskPointsDto
                    {
                        TaskId = task.Id,
                        Points = null
                    };
                var dto = mapper.Map<UserTaskPointsDto>(points);
                if (!points.Checked)
                    dto.Points = null;
                if (points is ManualReviewTaskUserAnswer manualReviewTaskUserAnswer)
                    dto.Content = manualReviewTaskUserAnswer.Content;
                return dto;
            }).ToList();

        return Results.Ok(pointsPrepared);
    }
}

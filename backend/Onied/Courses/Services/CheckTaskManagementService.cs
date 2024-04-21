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
    INotificationSentProducer notificationSentProducer)
    : ICheckTaskManagementService
{
    public async Task<Results<Ok<TasksBlock>, NotFound, ForbidHttpResult>> TryGetTaskBlock(
        Guid userId, int courseId, int blockId,
        bool includeVariants = false,
        bool includeAnswers = false)
    {
        var block = await blockRepository.GetTasksBlock(blockId, includeVariants, includeAnswers);

        if (block == null || block.Module.CourseId != courseId)
            return TypedResults.NotFound();

        if (await userCourseInfoRepository.GetUserCourseInfoAsync(userId, courseId) is null)
            return TypedResults.Forbid();

        return TypedResults.Ok(block);
    }

    public Results<Ok<List<UserTaskPoints>>, NotFound<string>, BadRequest<string>> GetUserTaskPoints(
        List<UserInputDto> inputsDto,
        TasksBlock block,
        Guid userId)
    {
        var points = new List<UserTaskPoints>();
        foreach (var inputDto in inputsDto)
        {
            var task = block.Tasks.SingleOrDefault(task => inputDto.TaskId == task.Id);

            if (task is null)
                return TypedResults.NotFound($"Task with id={inputDto.TaskId} not found.");

            if (task.TaskType != inputDto.TaskType)
                return TypedResults.BadRequest(
                    $"Task with id={inputDto.TaskId} has invalid TaskType={inputDto.TaskType}.");

            var tp = checkTasksService.CheckTask(task, inputDto);
            tp.UserId = userId;
            tp.CourseId = block.Module.CourseId;
            points.Add(tp);
        }

        return TypedResults.Ok(points);
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
                course.Title.Length > 100 ? string.Concat(course.Title.AsSpan(0, 97), "...") : course.Title,
                "Вы успешно закончили обучение на курсе!",
                userId,
                course.PictureHref);
            await notificationSentProducer.PublishForOne(notificationSent);
        }
    }
}

using Courses.Data.Models;
using Courses.Dtos.CheckTasks.Request;
using Courses.Services.Abstractions;
using Courses.Services.Producers.CourseCompletedProducer;
using Courses.Services.Producers.NotificationSentProducer;
using MassTransit.Data.Messages;
using Task = System.Threading.Tasks.Task;

namespace Courses.Services;

public class TaskCompletionService(
    ICheckTasksService checkTasksService,
    IBlockCompletedInfoRepository blockCompletedInfoRepository,
    ICourseRepository courseRepository,
    ICourseCompletedProducer courseCompletedProducer,
    INotificationSentProducer notificationSentProducer) : ITaskCompletionService
{
    public IResult GetUserTaskPoints(
        List<UserInputRequest> inputsDto,
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
        var bci = await blockCompletedInfoRepository.GetCompletedCourseBlockAsync(userId, blockId);
        if (pointsInfo.All(utp => utp.HasFullPoints) && bci is null)
        {
            await blockCompletedInfoRepository.AddCompletedCourseBlockAsync(userId, blockId);
        }
        else if (pointsInfo.Any(utp => !utp.HasFullPoints) && bci is not null)
        {
            await blockCompletedInfoRepository.DeleteCompletedCourseBlocksAsync(bci);
        }
    }

    public async Task ManageCourseCompleted(Guid userId, int courseId)
    {
        var course = (await courseRepository.GetCourseWithBlocksAsync(courseId))!;
        var courseBlocks = course.Modules
            .SelectMany(m => m.Blocks)
            .Where(b => b.BlockType == BlockType.TasksBlock)
            .Select(b => b.Id)
            .OrderBy(id => id);

        var userBlocks = (await blockCompletedInfoRepository
            .GetAllCompletedCourseBlocksByUser(userId, courseId))
            .Where(b => b.Block.BlockType == BlockType.TasksBlock)
            .Select(b => b.BlockId)
            .OrderBy(id => id);

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
}

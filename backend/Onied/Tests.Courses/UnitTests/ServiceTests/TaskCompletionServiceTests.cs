using Courses.Data.Models;
using Courses.Dtos.CheckTasks.Request;
using Courses.Services;
using Courses.Services.Abstractions;
using Courses.Services.Producers.CourseCompletedProducer;
using Courses.Services.Producers.NotificationSentProducer;
using MassTransit.Data.Messages;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using Block = Courses.Data.Models.Block;
using Task = System.Threading.Tasks.Task;
using TaskProj = Courses.Data.Models.Task;

namespace Tests.Courses.UnitTests.ServiceTests;

public class TaskCompletionServiceTests
{
    private readonly Mock<ICheckTasksService> _checkTasksService = new();
    private readonly Mock<IBlockCompletedInfoRepository> _blockCompletedInfoRepository = new();
    private readonly Mock<ICourseRepository> _courseRepository = new();
    private readonly Mock<ICourseCompletedProducer> _courseCompletedProducer = new();
    private readonly Mock<INotificationSentProducer> _notificationSentProducer = new();
    private readonly TaskCompletionService _taskCompletionService;

    public TaskCompletionServiceTests()
    {
        _taskCompletionService = new TaskCompletionService(
            _checkTasksService.Object,
            _blockCompletedInfoRepository.Object,
            _courseRepository.Object,
            _courseCompletedProducer.Object,
            _notificationSentProducer.Object);
    }

    [Fact]
    public void GetUserTaskPoints_TaskNotFound_ReturnsNotFound()
    {
        // Arrange
        var inputsDto = new List<UserInputRequest>
        {
            new UserInputRequest { TaskId = 1, TaskType = TaskType.SingleAnswer }
        };
        var block = new TasksBlock();
        var userId = Guid.NewGuid();

        // Act
        var result = _taskCompletionService.GetUserTaskPoints(inputsDto, block, userId);

        // Assert
        var notFoundResult = Assert.IsType<NotFound<string>>(result);
        Assert.Equal("Task with id=1 not found.", notFoundResult.Value);
    }

    [Fact]
    public void GetUserTaskPoints_InvalidTaskType_ReturnsBadRequest()
    {
        // Arrange
        var taskId = 1;
        var inputsDto = new List<UserInputRequest>
        {
            new UserInputRequest { TaskId = taskId, TaskType = TaskType.SingleAnswer }
        };
        var block = new TasksBlock();
        block.Tasks.Add(new TaskProj { Id = taskId, TaskType = TaskType.MultipleAnswers });
        var userId = Guid.NewGuid();

        // Act
        var result = _taskCompletionService.GetUserTaskPoints(inputsDto, block, userId);

        // Assert
        var badRequestResult = Assert.IsType<BadRequest<string>>(result);
        Assert.Equal($"Task with id={taskId} has invalid TaskType={TaskType.SingleAnswer}.", badRequestResult.Value);
    }

    [Fact]
    public async Task ManageTaskBlockCompleted_AllTasksCompleted_AddsBlockCompletion()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var blockId = 1;
        var pointsInfo = new List<UserTaskPoints>();

        // Act
        await _taskCompletionService.ManageTaskBlockCompleted(pointsInfo, userId, blockId);

        // Assert
        _blockCompletedInfoRepository.Verify(repo => repo.AddCompletedCourseBlockAsync(userId, blockId), Times.Once);
    }

    [Fact]
    public async Task ManageTaskBlockCompleted_NotAllTasksCompleted_RemovesBlockCompletion()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var blockId = 1;
        var pointsInfo = new List<UserTaskPoints>() { new UserTaskPoints() { Task = new TaskProj { MaxPoints = 1 } } };

        var completedBlock = new BlockCompletedInfo();
        _blockCompletedInfoRepository.Setup(repo => repo.GetCompletedCourseBlockAsync(userId, blockId))
            .ReturnsAsync(completedBlock);

        // Act
        await _taskCompletionService.ManageTaskBlockCompleted(pointsInfo, userId, blockId);

        // Assert
        _blockCompletedInfoRepository.Verify(repo => repo.DeleteCompletedCourseBlocksAsync(completedBlock), Times.Once);
    }

    [Fact]
    public async Task ManageCourseCompleted_AllBlocksCompleted_PublishesCourseCompleted()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var courseId = 1;
        var moduleId = 1;
        var blockId = 1;
        var course = new Course { Id = courseId };
        var module = new Module { Id = moduleId, Course = course, CourseId = courseId };
        var block = new Block { Id = blockId, Module = module, ModuleId = moduleId, BlockType = BlockType.TasksBlock };
        course.Modules.Add(module);
        module.Blocks.Add(block);

        _courseRepository.Setup(repo => repo.GetCourseWithBlocksAsync(courseId))
            .ReturnsAsync(course);

        _blockCompletedInfoRepository.Setup(repo => repo.GetAllCompletedCourseBlocksByUser(userId, courseId))
            .ReturnsAsync(new List<BlockCompletedInfo>
            {
                new BlockCompletedInfo { BlockId = 1, Block = new Block{ BlockType = BlockType.TasksBlock }}
            });

        // Act
        await _taskCompletionService.ManageCourseCompleted(userId, courseId);

        // Assert
        _courseCompletedProducer.Verify(repo => repo.PublishAsync(It.IsAny<CourseCompleted>()), Times.Once);
        _notificationSentProducer.Verify(repo => repo.PublishForOne(It.IsAny<NotificationSent>()), Times.Once);
    }

    [Fact]
    public async Task ManageCourseCompleted_NotAllBlocksCompleted_DoesNotPublish()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var courseId = 1;

        var course = new Course();

        _courseRepository.Setup(repo => repo.GetCourseWithBlocksAsync(courseId))
            .ReturnsAsync(course);

        _blockCompletedInfoRepository.Setup(repo => repo.GetAllCompletedCourseBlocksByUser(userId, courseId))
            .ReturnsAsync(new List<BlockCompletedInfo>
            {
                new BlockCompletedInfo { BlockId = 1, Block = new Block{ BlockType = BlockType.TasksBlock }}
            });

        // Act
        await _taskCompletionService.ManageCourseCompleted(userId, courseId);

        // Assert
        _courseCompletedProducer.Verify(repo => repo.PublishAsync(It.IsAny<CourseCompleted>()), Times.Never);
        _notificationSentProducer.Verify(repo => repo.PublishForOne(It.IsAny<NotificationSent>()), Times.Never);
    }
}

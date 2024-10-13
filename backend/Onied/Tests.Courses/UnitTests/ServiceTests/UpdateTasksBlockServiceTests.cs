using Courses.Data;
using Courses.Data.Models;
using Courses.Dtos.EditCourse.Request;
using Courses.Services;
using Courses.Services.Abstractions;
using Moq;
using Tests.Courses.Helpers;
using Task = System.Threading.Tasks.Task;

namespace Tests.Courses.UnitTests.ServiceTests;

public class UpdateTasksBlockServiceTests
{
    private readonly AppDbContext _dbContext;
    private readonly Mock<IBlockRepository> _blockRepository;
    private readonly UpdateTasksBlockService _service;

    public UpdateTasksBlockServiceTests()
    {
        _dbContext = AppDbContextTest.GetContext();
        _blockRepository = new Mock<IBlockRepository>();
        _service = new UpdateTasksBlockService(_dbContext, _blockRepository.Object);
    }

    [Fact]
    public async Task UpdateTasksBlock_RemovesExistingTasks()
    {
        // Arrange
        var tasksBlockId = 1;
        var tasksBlockRequest = new EditTasksBlockRequest { Id = tasksBlockId, Tasks = new List<EditTaskRequest>() };

        _dbContext.Tasks.Add(new() { TasksBlockId = tasksBlockId, Title = "title" });
        await _dbContext.SaveChangesAsync();

        // Act
        await _service.UpdateTasksBlock(tasksBlockRequest);

        // Assert
        Assert.Empty(_dbContext.Tasks.Where(t => t.TasksBlockId == tasksBlockId));
        _blockRepository.Verify(repo => repo.GetTasksBlock(tasksBlockId, true, true), Times.Once);
    }

    [Fact]
    public async Task UpdateTasksBlock_AddsSingleAnswerTask()
    {
        // Arrange
        var tasksBlockId = 1;
        var tasksBlockRequest = new EditTasksBlockRequest
        {
            Id = tasksBlockId,
            Tasks = new List<EditTaskRequest>
            {
                new EditTaskRequest
                {
                    TaskType = TaskType.SingleAnswer,
                    Title = "Sample Single Answer",
                    MaxPoints = 10,
                    Variants = new List<EditAnswersRequest>
                    {
                        new EditAnswersRequest { Description = "Option 1", IsCorrect = true },
                        new EditAnswersRequest { Description = "Option 2", IsCorrect = false }
                    }
                }
            }
        };

        _blockRepository.Setup(repo => repo.GetTasksBlock(tasksBlockId, true, true)).ReturnsAsync(new TasksBlock());

        // Act
        await _service.UpdateTasksBlock(tasksBlockRequest);

        // Assert
        Assert.Single(_dbContext.VariantsTasks);
        Assert.Equal("Sample Single Answer", _dbContext.VariantsTasks.First().Title);
        Assert.Equal(2, _dbContext.TaskVariants.Count());
        Assert.Equal("Option 1", _dbContext.TaskVariants.First().Description);
    }

    [Fact]
    public async Task UpdateTasksBlock_AddsMultipleAnswersTask()
    {
        // Arrange
        var tasksBlockId = 1;
        var tasksBlockRequest = new EditTasksBlockRequest
        {
            Id = tasksBlockId,
            Tasks = new List<EditTaskRequest>
            {
                new EditTaskRequest
                {
                    TaskType = TaskType.MultipleAnswers,
                    Title = "Sample Multiple Answers",
                    MaxPoints = 20,
                    Variants = new List<EditAnswersRequest>
                    {
                        new EditAnswersRequest { Description = "Option A", IsCorrect = true },
                        new EditAnswersRequest { Description = "Option B", IsCorrect = false }
                    }
                }
            }
        };

        _blockRepository.Setup(repo => repo.GetTasksBlock(tasksBlockId, true, true)).ReturnsAsync(new TasksBlock());

        // Act
        await _service.UpdateTasksBlock(tasksBlockRequest);

        // Assert
        Assert.Single(_dbContext.VariantsTasks);
        Assert.Equal("Sample Multiple Answers", _dbContext.VariantsTasks.First().Title);
        Assert.Equal(2, _dbContext.TaskVariants.Count());
    }

    [Fact]
    public async Task UpdateTasksBlock_AddsInputAnswerTask()
    {
        // Arrange
        var tasksBlockId = 1;
        var tasksBlockRequest = new EditTasksBlockRequest
        {
            Id = tasksBlockId,
            Tasks = new List<EditTaskRequest>
            {
                new EditTaskRequest
                {
                    TaskType = TaskType.InputAnswer,
                    Title = "Sample Input Answer",
                    MaxPoints = 15,
                    IsNumber = true,
                    Answers = new List<EditAnswersRequest>
                    {
                        new EditAnswersRequest { Answer = "42" }
                    }
                }
            }
        };

        _blockRepository.Setup(repo => repo.GetTasksBlock(tasksBlockId, true, true)).ReturnsAsync(new TasksBlock());

        // Act
        await _service.UpdateTasksBlock(tasksBlockRequest);

        // Assert
        Assert.Single(_dbContext.InputTasks);
        Assert.Equal("Sample Input Answer", _dbContext.InputTasks.First().Title);
        Assert.Single(_dbContext.TaskTextInputAnswers);
        Assert.Equal("42", _dbContext.TaskTextInputAnswers.First().Answer);
    }

    [Fact]
    public async Task UpdateTasksBlock_ReturnsUpdatedTasksBlock()
    {
        // Arrange
        var tasksBlockId = 1;
        var tasksBlockRequest = new EditTasksBlockRequest { Id = tasksBlockId, Tasks = new List<EditTaskRequest>() };
        var tasksBlock = new TasksBlock { Id = tasksBlockId };

        _blockRepository.Setup(repo => repo.GetTasksBlock(tasksBlockId, true, true)).ReturnsAsync(tasksBlock);

        // Act
        var result = await _service.UpdateTasksBlock(tasksBlockRequest);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(tasksBlockId, result.Id);
    }
}

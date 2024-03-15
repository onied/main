using AutoFixture;
using Courses.Dtos;
using Courses.Models;
using Courses.Services;
using Task = Courses.Models.Task;

namespace Tests.Courses.UnitTests.ServiceTests;

public class CheckTasksServiceTests
{
    private readonly ICheckTasksService _checkTasksService;
    private readonly Fixture _fixture = new();

    public CheckTasksServiceTests()
    {
        _checkTasksService = new CheckTasksService();
    }

    [Fact]
    public void CheckTask_NotIsDone()
    {
        // Arrange
        var input = _fixture.Build<UserInputDto>()
            .With(input1 => input1.IsDone, false)
            .Create();
        var expected = new UserTaskPoints()
        {
            UserId = input.UserId,
            TaskId = input.TaskId,
            Points = 0
        };
        
        // Act
        var actual = _checkTasksService.CheckTask(new Task(), input);
        
        // Assert
        Assert.Equivalent(expected, actual, true);
    }
    
    [Theory]
    [InlineData(1, true)]
    [InlineData(2, false)]
    public void CheckTask_MultipleAnswer(int variantId, bool isMaxPoints)
    {
        // Arrange
        var task = _fixture.Build<VariantsTask>()
            .With(task1 => task1.TaskType, TaskType.MultipleAnswers)
            .Create();
        var variants = _fixture.Build<TaskVariant>()
            .With(v => v.Id, 1)
            .With(v => v.IsCorrect, true)
            .CreateMany(1)
            .ToList();
        variants.ForEach(variant => task.Variants.Add(variant));
            
        var input = _fixture.Build<UserInputDto>()
            .With(input1 => input1.IsDone, true)
            .With(input1 => input1.VariantsIds, new List<int>{ variantId })
            .Create();
        var expected = new UserTaskPoints
        {
            UserId = input.UserId,
            TaskId = input.TaskId,
            Points = isMaxPoints ? task.MaxPoints : 0
        };
        
        // Act
        var actual = _checkTasksService.CheckTask(task, input);
        
        // Assert
        Assert.Equivalent(expected, actual, true);
    }
    
    [Theory]
    [InlineData(true, "Тест")]
    [InlineData(false, "тест")]
    public void CheckTask_InputAnswer_IsCaseSensitive(bool isCaseSensitive, string answer)
    {
        // Arrange
        var task = _fixture.Build<InputTask>()
            .With(task1 => task1.TaskType, TaskType.InputAnswer)
            .Do(task1 => task1.Answers.Add(new TaskTextInputAnswer
            {
                Answer = "Тест",
                IsCaseSensitive = isCaseSensitive
            }))
            .Create();
            
        var input = _fixture.Build<UserInputDto>()
            .With(input1 => input1.IsDone, true)
            .With(input1 => input1.Answer, answer)
            .Create();
        var expected = new UserTaskPoints
        {
            UserId = input.UserId,
            TaskId = input.TaskId,
            Points = task.MaxPoints
        };
        
        // Act
        var actual = _checkTasksService.CheckTask(task, input);
        
        // Assert
        Assert.Equivalent(expected, actual, true);
    }
}
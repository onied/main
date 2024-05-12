using AutoFixture;
using AutoMapper;
using Courses.Controllers;
using Courses.Dtos;
using Courses.Models;
using Courses.Profiles;
using Courses.Services;
using Courses.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Task = System.Threading.Tasks.Task;
using TaskProj = Courses.Models.Task;

namespace Tests.Courses.UnitTests.ControllerTests;

public class CheckTasksControllerTests
{
    private readonly Mock<IBlockRepository> _blockRepository = new();
    private readonly Mock<ICheckTasksService> _checkTasksService = new();
    private readonly Mock<IUserTaskPointsRepository> _userTaskPointsRepository = new();
    private readonly Mock<ICourseManagementService> _courseManagementService = new();
    private readonly Mock<CheckTaskManagementService> _checkTaskManagementService = new();
    private readonly CheckTasksController _controller;
    private readonly Fixture _fixture = new();
    private readonly Mock<ILogger<CoursesController>> _logger = new();

    private readonly IMapper _mapper =
        new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new AppMappingProfile())));

    public CheckTasksControllerTests()
    {
        _controller = new CheckTasksController(
            _logger.Object,
            _mapper,
            _userTaskPointsRepository.Object,
            _courseManagementService.Object,
            _checkTaskManagementService.Object);
    }

    [Fact]
    public async Task GetTaskPointsStored_ReturnsNotFound_WhenBlockNotExist()
    {
        // Arrange
        var courseId = -1;
        var blockId = -1;

        _blockRepository.Setup(b => b.GetTasksBlock(blockId, true, false))
            .Returns(Task.FromResult<TasksBlock?>(null));

        // Act
        var result = await _controller.GetTaskPointsStored(courseId, blockId, Guid.NewGuid(), null);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task GetTaskPointsStored_ReturnsNotFound_WhenCourseNotRight()
    {
        // Arrange
        var course = _fixture.Build<Course>()
            .With(c => c.IsProgramVisible, true)
            .Create();

        var module = _fixture.Build<Module>()
            .With(module => module.Course, course)
            .With(module => module.CourseId, course.Id)
            .Create();
        course.Modules.Add(module);

        var block = _fixture.Build<TasksBlock>()
            .With(block => block.Module, module)
            .With(block => block.ModuleId, module.Id)
            .With(block => block.BlockType, BlockType.TasksBlock)
            .Create();
        module.Blocks.Add(block);

        var courseId = -1;
        var blockId = block.Id;

        _blockRepository.Setup(b => b.GetTasksBlock(blockId, true, false))
            .Returns(Task.FromResult<TasksBlock?>(block));

        // Act
        var result = await _controller.GetTaskPointsStored(courseId, blockId, Guid.NewGuid(), null);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task GetTaskPointsStored_ReturnsNullBecauseManual()
    {
        // Arrange
        var course = _fixture.Build<Course>()
            .With(c => c.IsProgramVisible, true)
            .Create();

        var module = _fixture.Build<Module>()
            .With(module => module.Course, course)
            .With(module => module.CourseId, course.Id)
            .Create();
        course.Modules.Add(module);

        var block = _fixture.Build<TasksBlock>()
            .With(block => block.Module, module)
            .With(block => block.ModuleId, module.Id)
            .With(block => block.BlockType, BlockType.TasksBlock)
            .Create();
        module.Blocks.Add(block);

        var task = _fixture.Build<TaskProj>()
            .With(task => task.TasksBlock, block)
            .With(task => task.TasksBlockId, block.Id)
            .With(task => task.TaskType, TaskType.ManualReview)
            .Create();
        block.Tasks.Add(task);

        var courseId = course.Id;
        var blockId = block.Id;

        _blockRepository.Setup(b => b.GetTasksBlock(blockId, true, false))
            .Returns(Task.FromResult<TasksBlock?>(block));

        // Act
        var result = await _controller.GetTaskPointsStored(courseId, blockId, Guid.NewGuid(), null);

        // Assert
        var actionResult = Assert.IsType<ActionResult<List<UserTaskPointsDto>>>(result);
        var value = Assert.IsAssignableFrom<List<UserTaskPointsDto>>(
            actionResult.Value);
        Assert.All(value, Assert.Null);
    }

    [Fact]
    public async Task GetTaskPointsStored_ReturnsPointsRight()
    {
        // Arrange
        var course = _fixture.Build<Course>()
            .With(c => c.IsProgramVisible, true)
            .Create();

        var module = _fixture.Build<Module>()
            .With(module => module.Course, course)
            .With(module => module.CourseId, course.Id)
            .Create();
        course.Modules.Add(module);

        var block = _fixture.Build<TasksBlock>()
            .With(block => block.Module, module)
            .With(block => block.ModuleId, module.Id)
            .With(block => block.BlockType, BlockType.TasksBlock)
            .Create();
        module.Blocks.Add(block);

        var variantTasks = _fixture.Build<VariantsTask>()
            .With(task => task.TasksBlock, block)
            .With(task => task.TasksBlockId, block.Id)
            .With(task => task.TaskType, TaskType.MultipleAnswers)
            .CreateMany(3)
            .ToList();

        var courseId = course.Id;
        var blockId = block.Id;

        foreach (var variantTask in variantTasks)
        {
            block.Tasks.Add(variantTask);

            var variants = _fixture.Build<TaskVariant>()
                .With(variant => variant.Task, variantTask)
                .With(variant => variant.TaskId, variantTask.Id)
                .CreateMany(3)
                .ToList();

            variants.ForEach(variant => variantTask.Variants.Add(variant));
        }

        _blockRepository.Setup(b => b.GetTasksBlock(blockId, true, false))
            .Returns(Task.FromResult<TasksBlock?>(block));

        // Act
        var result = await _controller.GetTaskPointsStored(courseId, blockId, Guid.NewGuid(), null);

        // Assert
        var actionResult = Assert.IsType<ActionResult<List<UserTaskPointsDto>>>(result);
        var value = Assert.IsAssignableFrom<List<UserTaskPointsDto>>(
            actionResult.Value);
        Assert.Equivalent(block.Tasks.Select(task => task.Id),
            value.Select(x => x.TaskId));
        Assert.True(block.Tasks.Join(value,
                task => task.Id,
                dto => dto.TaskId,
                (task, dto) => (task.MaxPoints, dto.Points))
            .All(tuple => tuple.MaxPoints >= tuple.Points));
    }

    [Fact]
    public async Task CheckTaskBlock_ReturnsNotFound_WhenBlockNotExist()
    {
        // Arrange
        var courseId = -1;
        var blockId = -1;
        var userId = Guid.NewGuid();

        _blockRepository.Setup(b => b.GetTasksBlock(blockId, true, true))
            .Returns(Task.FromResult<TasksBlock?>(null));

        // Act
        var result = await _controller.CheckTaskBlock(courseId, blockId, userId, null, new List<UserInputDto>());

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task CheckTaskBlock_ReturnsNotFound_WhenCourseNotRight()
    {
        // Arrange
        var course = _fixture.Build<Course>()
            .With(c => c.IsProgramVisible, true)
            .Create();

        var module = _fixture.Build<Module>()
            .With(module => module.Course, course)
            .With(module => module.CourseId, course.Id)
            .Create();
        course.Modules.Add(module);

        var block = _fixture.Build<TasksBlock>()
            .With(block => block.Module, module)
            .With(block => block.ModuleId, module.Id)
            .With(block => block.BlockType, BlockType.TasksBlock)
            .Create();
        module.Blocks.Add(block);

        var courseId = -1;
        var blockId = block.Id;
        var userId = Guid.NewGuid();

        _blockRepository.Setup(b => b.GetTasksBlock(blockId, true, true))
            .Returns(Task.FromResult<TasksBlock?>(null));

        // Act
        var result = await _controller.CheckTaskBlock(courseId, blockId, userId, null, new List<UserInputDto>());

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task CheckTaskBlock_ReturnsBadRequest_EmptyAnyUserInputDto()
    {
        // Arrange
        var course = _fixture.Build<Course>()
            .With(c => c.IsProgramVisible, true)
            .Create();

        var module = _fixture.Build<Module>()
            .With(module => module.Course, course)
            .With(module => module.CourseId, course.Id)
            .Create();
        course.Modules.Add(module);

        var block = _fixture.Build<TasksBlock>()
            .With(block => block.Module, module)
            .With(block => block.ModuleId, module.Id)
            .With(block => block.BlockType, BlockType.TasksBlock)
            .Create();
        module.Blocks.Add(block);

        var courseId = course.Id;
        var blockId = block.Id;
        var userId = Guid.NewGuid();

        _blockRepository.Setup(b => b.GetTasksBlock(blockId, true, true))
            .Returns(Task.FromResult<TasksBlock?>(block));

        var inputsDto = _fixture.Build<UserInputDto>()
            .FromFactory(() => null)
            .CreateMany(1)
            .ToList();

        // Act
        var result = await _controller.CheckTaskBlock(courseId, blockId, userId, null, inputsDto);

        // Assert
        Assert.IsType<BadRequestResult>(result.Result);
    }

    [Fact]
    public async Task CheckTaskBlock_ReturnsNotFound_NotTaskInBlock()
    {
        // Arrange
        var course = _fixture.Build<Course>()
            .With(c => c.IsProgramVisible, true)
            .Create();

        var module = _fixture.Build<Module>()
            .With(module => module.Course, course)
            .With(module => module.CourseId, course.Id)
            .Create();
        course.Modules.Add(module);

        var block = _fixture.Build<TasksBlock>()
            .With(block => block.Module, module)
            .With(block => block.ModuleId, module.Id)
            .With(block => block.BlockType, BlockType.TasksBlock)
            .Create();
        module.Blocks.Add(block);

        var courseId = course.Id;
        var blockId = block.Id;
        var userId = Guid.NewGuid();

        _blockRepository.Setup(b => b.GetTasksBlock(blockId, true, true))
            .Returns(Task.FromResult<TasksBlock?>(block));

        var inputsDto = _fixture.Build<UserInputDto>()
            .With(input => input.TaskId, -1)
            .CreateMany(1)
            .ToList();

        // Act
        var result = await _controller.CheckTaskBlock(courseId, blockId, userId, null, inputsDto);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task CheckTaskBlock_ReturnsBadRequest_NotTaskType()
    {
        // Arrange
        var course = _fixture.Build<Course>()
            .With(c => c.IsProgramVisible, true)
            .Create();

        var module = _fixture.Build<Module>()
            .With(module => module.Course, course)
            .With(module => module.CourseId, course.Id)
            .Create();
        course.Modules.Add(module);

        var block = _fixture.Build<TasksBlock>()
            .With(block => block.Module, module)
            .With(block => block.ModuleId, module.Id)
            .With(block => block.BlockType, BlockType.TasksBlock)
            .Create();
        module.Blocks.Add(block);

        var task = _fixture.Build<InputTask>()
            .With(task => task.TasksBlock, block)
            .With(task => task.TasksBlockId, block.Id)
            .With(task => task.TaskType, TaskType.InputAnswer)
            .Create();
        block.Tasks.Add(task);

        var courseId = course.Id;
        var blockId = block.Id;
        var userId = Guid.NewGuid();

        _blockRepository.Setup(b => b.GetTasksBlock(blockId, true, true))
            .Returns(Task.FromResult<TasksBlock?>(block));

        var inputsDto = _fixture.Build<UserInputDto>()
            .With(input => input.TaskId, task.Id)
            .With(input => input.TaskType, task.TaskType + 1 % 4)
            .CreateMany(1)
            .ToList();

        // Act
        var result = await _controller.CheckTaskBlock(courseId, blockId, userId, null, inputsDto);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task CheckTaskBlock_ReturnsListUserTaskPointsDto()
    {
        // Arrange
        var course = _fixture.Build<Course>()
            .With(c => c.IsProgramVisible, true)
            .Create();

        var module = _fixture.Build<Module>()
            .With(module => module.Course, course)
            .With(module => module.CourseId, course.Id)
            .Create();
        course.Modules.Add(module);

        var block = _fixture.Build<TasksBlock>()
            .With(block => block.Module, module)
            .With(block => block.ModuleId, module.Id)
            .With(block => block.BlockType, BlockType.TasksBlock)
            .Create();
        module.Blocks.Add(block);

        var task = _fixture.Build<InputTask>()
            .With(task => task.TasksBlock, block)
            .With(task => task.TasksBlockId, block.Id)
            .With(task => task.TaskType, TaskType.InputAnswer)
            .Create();
        block.Tasks.Add(task);

        var courseId = course.Id;
        var blockId = block.Id;
        var userId = Guid.NewGuid();

        _blockRepository.Setup(b => b.GetTasksBlock(blockId, true, true))
            .Returns(Task.FromResult<TasksBlock?>(block));

        var inputsDto = _fixture.Build<UserInputDto>()
            .With(input => input.TaskId, task.Id)
            .With(input => input.TaskType, task.TaskType)
            .CreateMany(1)
            .ToList();

        _checkTasksService.Setup(cts => cts.CheckTask(It.IsAny<TaskProj>(), It.IsAny<UserInputDto>()))
            .Returns(new UserTaskPoints
            {
                TaskId = task.Id
            });

        // Act
        var result = await _controller.CheckTaskBlock(courseId, blockId, userId, null, inputsDto);

        // Assert
        var actionResult = Assert.IsType<ActionResult<List<UserTaskPointsDto>>>(result);
        var value = Assert.IsAssignableFrom<List<UserTaskPointsDto>>(
            actionResult.Value);
        Assert.Equivalent(task.Id,
            value.First().TaskId);
    }
}

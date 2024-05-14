using AutoFixture;
using AutoMapper;
using Courses.Dtos.CheckTasks.Request;
using Courses.Dtos.CheckTasks.Response;
using Courses.Models;
using Courses.Profiles;
using Courses.Services;
using Courses.Services.Abstractions;
using Courses.Services.Producers.CourseCompletedProducer;
using Courses.Services.Producers.NotificationSentProducer;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using Task = System.Threading.Tasks.Task;
using TaskProj = Courses.Models.Task;

namespace Tests.Courses.UnitTests.ServiceTests;

public class CheckTaskManagementServiceTests
{
    private readonly Mock<ICourseRepository> _courseRepository = new();
    private readonly Mock<IBlockRepository> _blockRepository = new();
    private readonly Mock<IBlockCompletedInfoRepository> _blockCompletedInfoRepository = new();
    private readonly Mock<IUserCourseInfoRepository> _userCourseInfoRepository = new();
    private readonly Mock<ICheckTasksService> _checkTasksService = new();
    private readonly Mock<IUserTaskPointsRepository> _userTaskPointsRepository = new();
    private readonly Mock<ICourseCompletedProducer> _courseCompletedProducer = new();
    private readonly Mock<ICourseManagementService> _courseManagementService = new();
    private readonly Mock<INotificationSentProducer> _notificationSentProducer = new();
    private readonly CheckTaskManagementService _service;
    private readonly Fixture _fixture = new();

    private readonly IMapper _mapper =
        new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new AppMappingProfile())));

    public CheckTaskManagementServiceTests()
    {
        _service = new CheckTaskManagementService(
            _courseRepository.Object,
            _blockRepository.Object,
            _blockCompletedInfoRepository.Object,
            _userCourseInfoRepository.Object,
            _checkTasksService.Object,
            _courseCompletedProducer.Object,
            _courseManagementService.Object,
            _userTaskPointsRepository.Object,
            _notificationSentProducer.Object,
            _mapper);
    }

    [Fact]
    public async Task GetTaskPointsStored_ReturnsNotFound_WhenBlockNotExist()
    {
        // Arrange
        var courseId = -1;
        var blockId = -1;

        _blockRepository.Setup(b => b.GetTasksBlock(blockId, true, false))
            .Returns(Task.FromResult<TasksBlock?>(null));
        _courseManagementService.Setup(x => x.AllowVisitCourse(It.IsAny<Guid>(), It.IsAny<int>()))
            .ReturnsAsync(true);

        // Act
        var result = await _service.GetTaskPointsStored(courseId, blockId, Guid.NewGuid());

        // Assert
        Assert.IsType<NotFound>(result);
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
        _courseManagementService.Setup(x => x.AllowVisitCourse(It.IsAny<Guid>(), It.IsAny<int>()))
            .ReturnsAsync(true);

        // Act
        var result = await _service.GetTaskPointsStored(courseId, blockId, Guid.NewGuid());

        // Assert
        Assert.IsType<NotFound>(result);
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
        _courseManagementService.Setup(x => x.AllowVisitCourse(It.IsAny<Guid>(), It.IsAny<int>()))
            .ReturnsAsync(true);
        _userCourseInfoRepository.Setup(x => x.GetUserCourseInfoAsync(It.IsAny<Guid>(), It.IsAny<int>(), false))
            .ReturnsAsync(new UserCourseInfo());
        _userTaskPointsRepository.Setup(x =>
                x.GetUserTaskPointsByUserAndBlock(It.IsAny<Guid>(), courseId, blockId))
            .ReturnsAsync(new List<UserTaskPoints>());

        // Act
        var result = await _service.GetTaskPointsStored(courseId, blockId, Guid.NewGuid());

        // Assert
        var actionResult = Assert.IsType<Ok<List<UserTaskPointsResponse>>>(result);
        var value = Assert.IsAssignableFrom<List<UserTaskPointsResponse>>(
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
        _courseManagementService.Setup(x => x.AllowVisitCourse(It.IsAny<Guid>(), It.IsAny<int>()))
            .ReturnsAsync(true);
        _userCourseInfoRepository.Setup(x => x.GetUserCourseInfoAsync(It.IsAny<Guid>(), It.IsAny<int>(), false))
            .ReturnsAsync(new UserCourseInfo());
        _userTaskPointsRepository.Setup(x =>
                x.GetUserTaskPointsByUserAndBlock(It.IsAny<Guid>(), courseId, blockId))
            .ReturnsAsync(new List<UserTaskPoints>());

        // Act
        var result = await _service.GetTaskPointsStored(courseId, blockId, Guid.NewGuid());

        // Assert
        var actionResult = Assert.IsType<Ok<List<UserTaskPointsResponse>>>(result);
        var value = Assert.IsAssignableFrom<List<UserTaskPointsResponse>>(
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
        _courseManagementService.Setup(x => x.AllowVisitCourse(It.IsAny<Guid>(), It.IsAny<int>()))
            .ReturnsAsync(true);

        // Act
        var result = await _service.CheckTaskBlock(courseId, blockId, userId, new List<UserInputRequest>());

        // Assert
        Assert.IsType<NotFound>(result);
    }

    [Fact]
    public async Task CheckTaskBlock_ReturnsNotFound_WhenCourseNotRight()
    {
        // Arrange
        var course = _fixture.Build<Course>()
            .With(c => c.IsProgramVisible, true)
            .Create();

        _courseManagementService.Setup(x => x.AllowVisitCourse(It.IsAny<Guid>(), It.IsAny<int>()))
            .ReturnsAsync(true);

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
        var result = await _service.CheckTaskBlock(courseId, blockId, userId, new List<UserInputRequest>());

        // Assert
        Assert.IsType<NotFound>(result);
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
        _courseManagementService.Setup(x => x.AllowVisitCourse(It.IsAny<Guid>(), It.IsAny<int>()))
            .ReturnsAsync(true);
        _userCourseInfoRepository.Setup(x => x.GetUserCourseInfoAsync(It.IsAny<Guid>(), It.IsAny<int>(), false))
            .ReturnsAsync(new UserCourseInfo());
        _userTaskPointsRepository.Setup(x =>
                x.GetUserTaskPointsByUserAndBlock(It.IsAny<Guid>(), courseId, blockId))
            .ReturnsAsync(new List<UserTaskPoints>());
        _checkTasksService.Setup(cts => cts.CheckTask(It.IsAny<TaskProj>(), It.IsAny<UserInputRequest>()))
            .Returns(new UserTaskPoints());

        var inputsDto = _fixture.Build<UserInputRequest>()
            .FromFactory(() => null)
            .CreateMany(1)
            .ToList();

        // Act
        var result = await _service.CheckTaskBlock(courseId, blockId, userId, inputsDto);

        // Assert
        Assert.IsType<BadRequest>(result);
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
        _courseManagementService.Setup(x => x.AllowVisitCourse(It.IsAny<Guid>(), It.IsAny<int>()))
            .ReturnsAsync(true);
        _userCourseInfoRepository.Setup(x => x.GetUserCourseInfoAsync(It.IsAny<Guid>(), It.IsAny<int>(), false))
            .ReturnsAsync(new UserCourseInfo());
        _userTaskPointsRepository.Setup(x =>
                x.GetUserTaskPointsByUserAndBlock(It.IsAny<Guid>(), courseId, blockId))
            .ReturnsAsync(new List<UserTaskPoints>());

        var inputsDto = _fixture.Build<UserInputRequest>()
            .With(input => input.TaskId, -1)
            .CreateMany(1)
            .ToList();

        // Act
        var result = await _service.CheckTaskBlock(courseId, blockId, userId, inputsDto);

        // Assert
        Assert.IsType<NotFound<string>>(result);
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
        _courseManagementService.Setup(x => x.AllowVisitCourse(It.IsAny<Guid>(), It.IsAny<int>()))
            .ReturnsAsync(true);
        _userCourseInfoRepository.Setup(x => x.GetUserCourseInfoAsync(It.IsAny<Guid>(), It.IsAny<int>(), false))
            .ReturnsAsync(new UserCourseInfo());
        _userTaskPointsRepository.Setup(x =>
                x.GetUserTaskPointsByUserAndBlock(It.IsAny<Guid>(), courseId, blockId))
            .ReturnsAsync(new List<UserTaskPoints>());

        var inputsDto = _fixture.Build<UserInputRequest>()
            .With(input => input.TaskId, task.Id)
            .With(input => input.TaskType, task.TaskType + 1 % 4)
            .CreateMany(1)
            .ToList();

        // Act
        var result = await _service.CheckTaskBlock(courseId, blockId, userId, inputsDto);

        // Assert
        Assert.IsType<BadRequest<string>>(result);
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
        _courseManagementService.Setup(x => x.AllowVisitCourse(It.IsAny<Guid>(), It.IsAny<int>()))
            .ReturnsAsync(true);
        _userCourseInfoRepository.Setup(x => x.GetUserCourseInfoAsync(It.IsAny<Guid>(), It.IsAny<int>(), false))
            .ReturnsAsync(new UserCourseInfo());
        _userTaskPointsRepository.Setup(x =>
                x.GetUserTaskPointsByUserAndBlock(It.IsAny<Guid>(), courseId, blockId))
            .ReturnsAsync(new List<UserTaskPoints>());
        _blockCompletedInfoRepository.Setup(x => x
                .GetCompletedCourseBlockAsync(userId, blockId))
            .ReturnsAsync(_fixture.Create<BlockCompletedInfo>());

        var inputsDto = _fixture.Build<UserInputRequest>()
            .With(input => input.TaskId, task.Id)
            .With(input => input.TaskType, task.TaskType)
            .CreateMany(1)
            .ToList();

        _checkTasksService.Setup(cts => cts.CheckTask(It.IsAny<TaskProj>(), It.IsAny<UserInputRequest>()))
            .Returns(new UserTaskPoints
            {
                TaskId = task.Id
            });

        // Act
        var result = await _service.CheckTaskBlock(courseId, blockId, userId, inputsDto);

        // Assert
        var actionResult = Assert.IsType<Ok<List<UserTaskPointsResponse>>>(result);
        var value = Assert.IsAssignableFrom<List<UserTaskPointsResponse>>(
            actionResult.Value);
        Assert.Equivalent(task.Id,
            value.First().TaskId);
    }
}

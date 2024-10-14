using AutoFixture;
using AutoMapper;
using Courses.Data.Models;
using Courses.Dtos.CheckTasks.Request;
using Courses.Dtos.CheckTasks.Response;
using Courses.Profiles;
using Courses.Services;
using Courses.Services.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using Task = System.Threading.Tasks.Task;
using TaskProj = Courses.Data.Models.Task;

namespace Tests.Courses.UnitTests.ServiceTests;

public class CheckTaskManagementServiceTests
{
    private readonly Mock<IBlockRepository> _blockRepository = new();
    private readonly Mock<IUserCourseInfoRepository> _userCourseInfoRepository = new();
    private readonly Mock<IUserTaskPointsRepository> _userTaskPointsRepository = new();
    private readonly Mock<ICourseManagementService> _courseManagementService = new();
    private readonly Mock<ITaskCompletionService> _taskCompletionService = new();
    private readonly CheckTaskManagementService _service;
    private readonly Fixture _fixture = new();

    private readonly IMapper _mapper =
        new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new AppMappingProfile())));

    public CheckTaskManagementServiceTests()
    {
        _service = new CheckTaskManagementService(
            _blockRepository.Object,
            _userCourseInfoRepository.Object,
            _taskCompletionService.Object,
            _courseManagementService.Object,
            _userTaskPointsRepository.Object,
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
        _courseManagementService.Setup(x => x.AllowVisitCourse(It.IsAny<Guid>(), It.IsAny<int>(), null))
            .ReturnsAsync(true);

        // Act
        var result = await _service.GetTaskPointsStored(courseId, blockId, Guid.NewGuid(), null);

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
        _courseManagementService.Setup(x => x.AllowVisitCourse(It.IsAny<Guid>(), It.IsAny<int>(), null))
            .ReturnsAsync(true);

        // Act
        var result = await _service.GetTaskPointsStored(courseId, blockId, Guid.NewGuid(), null);

        // Assert
        Assert.IsType<NotFound>(result);
    }

    [Fact]
    public async Task GetTaskPointsStored_ReturnsNullBecauseManual()
    {
        // Arrange
        var courseId = 1;
        var moduleId = 1;
        var blockId = 1;
        var course = new Course { Id = courseId };
        var module = new Module { Id = moduleId, CourseId = courseId };
        var block = new TasksBlock { Id = blockId, ModuleId = moduleId, Module = module };
        course.Modules.Add(module);
        module.Blocks.Add(block);

        var task = new TaskProj { TasksBlock = block, TasksBlockId = blockId, TaskType = TaskType.ManualReview };
        block.Tasks.Add(task);

        _blockRepository.Setup(b => b.GetTasksBlock(blockId, true, false))
            .ReturnsAsync(block);
        _courseManagementService.Setup(x => x.AllowVisitCourse(It.IsAny<Guid>(), It.IsAny<int>(), null))
            .ReturnsAsync(true);
        _userCourseInfoRepository.Setup(x => x.GetUserCourseInfoAsync(It.IsAny<Guid>(), It.IsAny<int>(), false))
            .ReturnsAsync(new UserCourseInfo());
        _userTaskPointsRepository.Setup(x =>
                x.GetUserTaskPointsByUserAndBlock(It.IsAny<Guid>(), courseId, blockId))
            .ReturnsAsync(new List<UserTaskPoints>());

        // Act
        var result = await _service.GetTaskPointsStored(courseId, blockId, Guid.NewGuid(), null);

        // Assert
        var actionResult = Assert.IsType<Ok<List<UserTaskPointsResponse>>>(result);
        var value = Assert.IsAssignableFrom<List<UserTaskPointsResponse>>(
            actionResult.Value);
        Assert.Equivalent(value[0].TaskId, task.Id);
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
        _courseManagementService.Setup(x => x.AllowVisitCourse(It.IsAny<Guid>(), It.IsAny<int>(), null))
            .ReturnsAsync(true);
        _userCourseInfoRepository.Setup(x => x.GetUserCourseInfoAsync(It.IsAny<Guid>(), It.IsAny<int>(), false))
            .ReturnsAsync(new UserCourseInfo());
        _userTaskPointsRepository.Setup(x =>
                x.GetUserTaskPointsByUserAndBlock(It.IsAny<Guid>(), courseId, blockId))
            .ReturnsAsync(new List<UserTaskPoints>());

        // Act
        var result = await _service.GetTaskPointsStored(courseId, blockId, Guid.NewGuid(), null);

        // Assert
        var actionResult = Assert.IsType<Ok<List<UserTaskPointsResponse>>>(result);
        var value = Assert.IsAssignableFrom<List<UserTaskPointsResponse>>(
            actionResult.Value);
        Assert.Equivalent(block.Tasks.Select(task => task.Id),
            value.Select(x => x.TaskId));
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
        _courseManagementService.Setup(x => x.AllowVisitCourse(It.IsAny<Guid>(), It.IsAny<int>(), null))
            .ReturnsAsync(true);

        // Act
        var result = await _service.CheckTaskBlock(courseId, blockId, userId, null, new List<UserInputRequest>());

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

        _courseManagementService.Setup(x => x.AllowVisitCourse(It.IsAny<Guid>(), It.IsAny<int>(), null))
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
        var result = await _service.CheckTaskBlock(courseId, blockId, userId, null, new List<UserInputRequest>());

        // Assert
        Assert.IsType<NotFound>(result);
    }

    [Fact]
    public async Task CheckTaskBlock_ReturnsOk()
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
        _courseManagementService.Setup(x => x.AllowVisitCourse(It.IsAny<Guid>(), It.IsAny<int>(), null))
            .ReturnsAsync(true);
        _userCourseInfoRepository.Setup(x => x.GetUserCourseInfoAsync(It.IsAny<Guid>(), It.IsAny<int>(), false))
            .ReturnsAsync(new UserCourseInfo());
        _userTaskPointsRepository.Setup(x =>
                x.GetUserTaskPointsByUserAndBlock(It.IsAny<Guid>(), courseId, blockId))
            .ReturnsAsync(new List<UserTaskPoints>());
        _taskCompletionService.Setup(t =>
                t.GetUserTaskPoints(It.IsAny<List<UserInputRequest>>(), It.IsAny<TasksBlock>(), It.IsAny<Guid>()))
            .Returns(Results.Ok());

        var inputsDto = _fixture.Build<UserInputRequest>()
            .FromFactory(() => null!)
            .CreateMany(1)
            .ToList();

        // Act
        var result = await _service.CheckTaskBlock(courseId, blockId, userId, null, inputsDto);

        // Assert
        Assert.IsType<Ok>(result);
    }

    [Fact]
    public async Task CheckTaskBlock_ReturnsOk_UserTaskPointsResponse()
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
        block.Tasks.Add(new TaskProj());

        var courseId = course.Id;
        var blockId = block.Id;
        var userId = Guid.NewGuid();

        _blockRepository.Setup(b => b.GetTasksBlock(blockId, true, true))
            .ReturnsAsync(block);
        _courseManagementService.Setup(x => x.AllowVisitCourse(It.IsAny<Guid>(), It.IsAny<int>(), null))
            .ReturnsAsync(true);
        _userCourseInfoRepository.Setup(x => x.GetUserCourseInfoAsync(It.IsAny<Guid>(), It.IsAny<int>(), false))
            .ReturnsAsync(new UserCourseInfo());
        _userTaskPointsRepository.Setup(x =>
                x.GetUserTaskPointsByUserAndBlock(It.IsAny<Guid>(), courseId, blockId))
            .ReturnsAsync(new List<UserTaskPoints>());
        _taskCompletionService.Setup(t =>
                t.GetUserTaskPoints(It.IsAny<List<UserInputRequest>>(), It.IsAny<TasksBlock>(), It.IsAny<Guid>()))
            .Returns(Results.Ok(new List<UserTaskPoints>()));

        var inputsDto = _fixture.Build<UserInputRequest>()
            .FromFactory(() => null!)
            .CreateMany(1)
            .ToList();

        // Act
        var result = await _service.CheckTaskBlock(courseId, blockId, userId, null, inputsDto);

        // Assert
        Assert.IsType<Ok<List<UserTaskPointsResponse>>>(result);
    }
}

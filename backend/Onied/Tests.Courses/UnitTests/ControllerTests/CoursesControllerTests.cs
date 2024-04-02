using AutoFixture;
using AutoMapper;
using Courses.Controllers;
using Courses.Dtos;
using Courses.Models;
using Courses.Profiles;
using Courses.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Task = System.Threading.Tasks.Task;
using TaskProj = Courses.Models.Task;

namespace Tests.Courses.UnitTests.ControllerTests;

public class CoursesControllerTests
{
    private readonly Mock<IBlockRepository> _blockRepository = new();
    private readonly Mock<ICategoryRepository> _categoryRepository = new();
    private readonly Mock<ICheckTasksService> _checkTasksService = new();
    private readonly CoursesController _controller;
    private readonly Mock<ICourseRepository> _courseRepository = new();
    private readonly Fixture _fixture = new();
    private readonly Mock<ILogger<CoursesController>> _logger = new();

    private readonly IMapper _mapper =
        new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new AppMappingProfile())));

    public CoursesControllerTests()
    {
        _controller = new CoursesController(
            _logger.Object,
            _mapper,
            _courseRepository.Object,
            _blockRepository.Object,
            _checkTasksService.Object, _categoryRepository.Object);
    }

    [Fact]
    public async Task GetPreviewCourse_ReturnsNotFound_WhenCourseNotExist()
    {
        // Arrange
        var courseId = -1;

        _courseRepository.Setup(r => r.GetCourseAsync(-1))
            .Returns(Task.FromResult<Course?>(null));

        // Act
        var result = await _controller.GetCoursePreview(courseId);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task GetCoursePreview_ReturnsCoursePreview_NotProgramVisible()
    {
        // Arrange
        var course = _fixture.Build<Course>()
            .With(c => c.IsProgramVisible, false)
            .Create();
        var courseId = course.Id;

        var author = _fixture.Build<User>()
            .With(author1 => author1.Id, Guid.NewGuid)
            .Do(author1 => author1.Courses.Add(course))
            .Create();
        course.Author = author;
        course.AuthorId = author.Id;

        var sequenceModule = 1;
        var modules = _fixture.Build<Module>()
            .With(module => module.Id, () => sequenceModule++)
            .With(module => module.Course, course)
            .With(module => module.CourseId, course.Id)
            .CreateMany(3)
            .ToList();
        modules.ForEach(module => course.Modules.Add(module));

        _courseRepository.Setup(r => r.GetCourseAsync(courseId))
            .Returns(Task.FromResult<Course?>(course));

        // Act
        var result = await _controller.GetCoursePreview(courseId);

        // Assert
        var actionResult = Assert.IsType<ActionResult<PreviewDto>>(result);
        var value = Assert.IsAssignableFrom<PreviewDto>(
            actionResult.Value);
        Assert.Equal(courseId, value.Id);
        Assert.Equivalent(_mapper.Map<AuthorDto>(course.Author), value.CourseAuthor, true);
    }

    [Fact]
    public async Task GetCoursePreview_ReturnsCoursePreview_ProgramVisible()
    {
        // Arrange
        var course = _fixture.Build<Course>()
            .With(c => c.IsProgramVisible, true)
            .Create();
        var courseId = course.Id;

        var author = _fixture.Build<User>()
            .With(author1 => author1.Id, Guid.NewGuid)
            .Do(author1 => author1.Courses.Add(course))
            .Create();
        course.Author = author;
        course.AuthorId = author.Id;

        var sequenceModule = 1;
        var modules = _fixture.Build<Module>()
            .With(module => module.Id, () => sequenceModule++)
            .With(module => module.Course, course)
            .With(module => module.CourseId, course.Id)
            .CreateMany(3)
            .ToList();
        modules.ForEach(module => course.Modules.Add(module));

        _courseRepository.Setup(r => r.GetCourseAsync(courseId))
            .Returns(Task.FromResult<Course?>(course));

        // Act
        var result = await _controller.GetCoursePreview(courseId);

        // Assert
        var actionResult = Assert.IsType<ActionResult<PreviewDto>>(result);
        var value = Assert.IsAssignableFrom<PreviewDto>(
            actionResult.Value);
        Assert.Equal(courseId, value.Id);
        Assert.Equivalent(course.Modules.Select(module => module.Title), value.CourseProgram);
    }

    [Fact]
    public async Task GetCourseHierarchy_ReturnsNotFound_WhenCourseNotExist()
    {
        // Arrange
        var courseId = -1;

        _courseRepository.Setup(r => r.GetCourseWithBlocksAsync(courseId))
            .Returns(Task.FromResult<Course?>(null));

        // Act
        var result = await _controller.GetCourseHierarchy(courseId);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task GetCourseHierarchy_ReturnsCourse()
    {
        // Arrange
        var course = _fixture.Build<Course>()
            .With(c => c.IsProgramVisible, true)
            .Create();
        var courseId = course.Id;

        var sequenceModule = 1;
        var modules = _fixture.Build<Module>()
            .With(module => module.Id, () => sequenceModule++)
            .With(module => module.Course, course)
            .With(module => module.CourseId, course.Id)
            .CreateMany(3)
            .ToList();
        modules.ForEach(module => course.Modules.Add(module));

        _courseRepository.Setup(r => r.GetCourseWithBlocksAsync(courseId))
            .Returns(Task.FromResult<Course?>(course));

        // Act
        var result = await _controller.GetCourseHierarchy(courseId);

        // Assert
        var actionResult = Assert.IsType<ActionResult<CourseDto>>(result);
        var value = Assert.IsAssignableFrom<CourseDto>(
            actionResult.Value);
        Assert.Equal(courseId, value.Id);
        Assert.Equal(course.Modules.Count, value.Modules.Count);
    }

    [Fact]
    public async Task GetSummaryBlock_ReturnsNotFound_WhenBlockNotExist()
    {
        // Arrange
        var courseId = -1;
        var blockId = -1;

        _blockRepository.Setup(b => b.GetSummaryBlock(blockId))
            .Returns(Task.FromResult<SummaryBlock?>(null));

        // Act
        var result = await _controller.GetSummaryBlock(courseId, blockId);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task GetSummaryBlock_ReturnsNotFound_WhenCourseNotRight()
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

        var block = _fixture.Build<SummaryBlock>()
            .With(block => block.Module, module)
            .With(block => block.ModuleId, module.Id)
            .With(block => block.BlockType, BlockType.SummaryBlock)
            .Create();
        module.Blocks.Add(block);

        var courseId = -1;
        var blockId = block.Id;

        _blockRepository.Setup(b => b.GetSummaryBlock(blockId))
            .Returns(Task.FromResult<SummaryBlock?>(block));

        // Act
        var result = await _controller.GetSummaryBlock(courseId, blockId);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task GetSummaryBlock_ReturnsBlock()
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

        var block = _fixture.Build<SummaryBlock>()
            .With(block => block.Module, module)
            .With(block => block.ModuleId, module.Id)
            .With(block => block.BlockType, BlockType.SummaryBlock).Create();
        module.Blocks.Add(block);

        var courseId = course.Id;
        var blockId = block.Id;

        _blockRepository.Setup(b => b.GetSummaryBlock(blockId))
            .Returns(Task.FromResult<SummaryBlock?>(block));

        // Act
        var result = await _controller.GetSummaryBlock(courseId, blockId);

        // Assert
        var actionResult = Assert.IsType<ActionResult<SummaryBlockDto>>(result);
        var value = Assert.IsAssignableFrom<SummaryBlockDto>(
            actionResult.Value);

        Assert.Equal(blockId, value.Id);
    }

    [Fact]
    public async Task GetVideoBlock_ReturnsNotFound_WhenBlockNotExist()
    {
        var courseId = -1;
        var blockId = -1;

        _blockRepository.Setup(b => b.GetSummaryBlock(blockId))
            .Returns(Task.FromResult<SummaryBlock?>(null));

        // Act
        var result = await _controller.GetVideoBlock(courseId, blockId);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task GetVideoBlock_ReturnsNotFound_WhenCourseNotRight()
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

        var block = _fixture.Build<VideoBlock>()
            .With(block => block.Module, module)
            .With(block => block.ModuleId, module.Id)
            .With(block => block.BlockType, BlockType.VideoBlock).Create();
        module.Blocks.Add(block);

        var courseId = -1;
        var blockId = block.Id;

        _blockRepository.Setup(b => b.GetVideoBlock(blockId))
            .Returns(Task.FromResult<VideoBlock?>(block));

        // Act
        var result = await _controller.GetVideoBlock(courseId, blockId);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task GetVideoBlock_ReturnsBlock()
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

        var block = _fixture.Build<VideoBlock>()
            .With(block => block.Module, module)
            .With(block => block.ModuleId, module.Id)
            .With(block => block.BlockType, BlockType.VideoBlock)
            .Create();
        module.Blocks.Add(block);

        var courseId = course.Id;
        var blockId = block.Id;

        _blockRepository.Setup(b => b.GetVideoBlock(blockId))
            .Returns(Task.FromResult<VideoBlock?>(block));

        // Act
        var result = await _controller.GetVideoBlock(courseId, blockId);

        // Assert
        var actionResult = Assert.IsType<ActionResult<VideoBlockDto>>(result);
        var value = Assert.IsAssignableFrom<VideoBlockDto>(
            actionResult.Value);
        Assert.Equal(blockId, value.Id);
    }

    [Fact]
    public async Task GetTasksBlock_ReturnsNotFound_WhenBlockNotExist()
    {
        // Arrange
        var courseId = -1;
        var blockId = -1;

        _blockRepository.Setup(b => b.GetTasksBlock(blockId, true, false))
            .Returns(Task.FromResult<TasksBlock?>(null));

        // Act
        var result = await _controller.GetTaskBlock(courseId, blockId);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task GetTasksBlock_ReturnsNotFound_WhenCourseNotRight()
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
        var result = await _controller.GetTaskBlock(courseId, blockId);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task GetTasksBlock_ReturnsBlock()
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

        _blockRepository.Setup(b => b.GetTasksBlock(blockId, true, false))
            .Returns(Task.FromResult<TasksBlock?>(block));

        // Act
        var result = await _controller.GetTaskBlock(courseId, blockId);

        // Assert
        var actionResult = Assert.IsType<ActionResult<TasksBlockDto>>(result);
        var value = Assert.IsAssignableFrom<TasksBlockDto>(
            actionResult.Value);
        Assert.Equal(blockId, value.Id);
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
        var result = await _controller.GetTaskPointsStored(courseId, blockId);

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
        var result = await _controller.GetTaskPointsStored(courseId, blockId);

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
        var result = await _controller.GetTaskPointsStored(courseId, blockId);

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
        var result = await _controller.GetTaskPointsStored(courseId, blockId);

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

        _blockRepository.Setup(b => b.GetTasksBlock(blockId, true, true))
            .Returns(Task.FromResult<TasksBlock?>(null));

        // Act
        var result = await _controller.CheckTaskBlock(courseId, blockId, new List<UserInputDto>());

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

        _blockRepository.Setup(b => b.GetTasksBlock(blockId, true, true))
            .Returns(Task.FromResult<TasksBlock?>(null));

        // Act
        var result = await _controller.CheckTaskBlock(courseId, blockId, new List<UserInputDto>());

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

        _blockRepository.Setup(b => b.GetTasksBlock(blockId, true, true))
            .Returns(Task.FromResult<TasksBlock?>(block));

        var inputsDto = _fixture.Build<UserInputDto>()
            .FromFactory(() => null)
            .CreateMany(1)
            .ToList();

        // Act
        var result = await _controller.CheckTaskBlock(courseId, blockId, inputsDto);

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

        _blockRepository.Setup(b => b.GetTasksBlock(blockId, true, true))
            .Returns(Task.FromResult<TasksBlock?>(block));

        var inputsDto = _fixture.Build<UserInputDto>()
            .With(input => input.TaskId, -1)
            .CreateMany(1)
            .ToList();

        // Act
        var result = await _controller.CheckTaskBlock(courseId, blockId, inputsDto);

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

        _blockRepository.Setup(b => b.GetTasksBlock(blockId, true, true))
            .Returns(Task.FromResult<TasksBlock?>(block));

        var inputsDto = _fixture.Build<UserInputDto>()
            .With(input => input.TaskId, task.Id)
            .With(input => input.TaskType, task.TaskType + 1 % 4)
            .CreateMany(1)
            .ToList();

        // Act
        var result = await _controller.CheckTaskBlock(courseId, blockId, inputsDto);

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
        var result = await _controller.CheckTaskBlock(courseId, blockId, inputsDto);

        // Assert
        var actionResult = Assert.IsType<ActionResult<List<UserTaskPointsDto>>>(result);
        var value = Assert.IsAssignableFrom<List<UserTaskPointsDto>>(
            actionResult.Value);
        Assert.Equivalent(task.Id,
            value.First().TaskId);
    }
}

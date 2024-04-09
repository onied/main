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

namespace Tests.Courses.UnitTests.ControllerTests;

public class CoursesControllerTests
{
    private readonly Mock<ILogger<CoursesController>> _logger = new();
    private readonly IMapper _mapper =
        new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new AppMappingProfile())));
    private readonly Mock<ICategoryRepository> _categoryRepository = new();
    private readonly Mock<ICourseRepository> _courseRepository = new();
    private readonly Mock<IBlockRepository> _blockRepository = new();
    private readonly Mock<IBlockCompletedInfoRepository> _blockCompletedInfoRepository = new();
    private readonly Mock<IUserRepository> _userRepository = new();
    private readonly CoursesController _controller;
    private readonly Fixture _fixture = new();

    public CoursesControllerTests()
    {
        _controller = new CoursesController(
            _logger.Object,
            _mapper,
            _courseRepository.Object,
            _blockRepository.Object,
            _categoryRepository.Object,
            _userRepository.Object,
            _blockCompletedInfoRepository.Object);
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
        var userId = Guid.NewGuid();

        _courseRepository.Setup(r => r.GetCourseWithBlocksAsync(courseId))
            .Returns(Task.FromResult<Course?>(null));

        // Act
        var result = await _controller.GetCourseHierarchy(courseId, userId);

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
        var userId = Guid.NewGuid();

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
        _blockCompletedInfoRepository
            .Setup(r => r.GetAllCompletedCourseBlocksByUser(userId, courseId))
            .Returns(Task.FromResult<List<BlockCompletedInfo>>([]));

        // Act
        var result = await _controller.GetCourseHierarchy(courseId, userId);

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
        var userId = Guid.NewGuid();

        _blockRepository.Setup(b => b.GetSummaryBlock(blockId))
            .Returns(Task.FromResult<SummaryBlock?>(null));

        // Act
        var result = await _controller.GetSummaryBlock(courseId, blockId, userId);

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
        var userId = Guid.NewGuid();

        _blockRepository.Setup(b => b.GetSummaryBlock(blockId))
            .Returns(Task.FromResult<SummaryBlock?>(block));

        // Act
        var result = await _controller.GetSummaryBlock(courseId, blockId, userId);

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
        var userId = Guid.NewGuid();

        _blockRepository.Setup(b => b.GetSummaryBlock(blockId))
            .Returns(Task.FromResult<SummaryBlock?>(block));

        // Act
        var result = await _controller.GetSummaryBlock(courseId, blockId, userId);

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
        var userId = Guid.NewGuid();

        _blockRepository.Setup(b => b.GetSummaryBlock(blockId))
            .Returns(Task.FromResult<SummaryBlock?>(null));

        // Act
        var result = await _controller.GetVideoBlock(courseId, blockId, userId);

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
        var userId = Guid.NewGuid();

        _blockRepository.Setup(b => b.GetVideoBlock(blockId))
            .Returns(Task.FromResult<VideoBlock?>(block));

        // Act
        var result = await _controller.GetVideoBlock(courseId, blockId, userId);

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
        var userId = Guid.NewGuid();

        _blockRepository.Setup(b => b.GetVideoBlock(blockId))
            .Returns(Task.FromResult<VideoBlock?>(block));

        // Act
        var result = await _controller.GetVideoBlock(courseId, blockId, userId);

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
        var userId = Guid.NewGuid();

        _blockRepository.Setup(b => b.GetTasksBlock(blockId, true, false))
            .Returns(Task.FromResult<TasksBlock?>(null));

        // Act
        var result = await _controller.GetTaskBlock(courseId, blockId, userId);

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
        var userId = Guid.NewGuid();

        _blockRepository.Setup(b => b.GetTasksBlock(blockId, true, false))
            .Returns(Task.FromResult<TasksBlock?>(block));

        // Act
        var result = await _controller.GetTaskBlock(courseId, blockId, userId);

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
        var userId = Guid.NewGuid();

        _blockRepository.Setup(b => b.GetTasksBlock(blockId, true, false))
            .Returns(Task.FromResult<TasksBlock?>(block));

        // Act
        var result = await _controller.GetTaskBlock(courseId, blockId, userId);

        // Assert
        var actionResult = Assert.IsType<ActionResult<TasksBlockDto>>(result);
        var value = Assert.IsAssignableFrom<TasksBlockDto>(
            actionResult.Value);
        Assert.Equal(blockId, value.Id);
    }
}

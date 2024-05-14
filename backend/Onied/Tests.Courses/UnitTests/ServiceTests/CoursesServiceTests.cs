using AutoFixture;
using AutoMapper;
using Courses.Dtos.Catalog.Response;
using Courses.Dtos.Course.Response;
using Courses.Models;
using Courses.Profiles;
using Courses.Services;
using Courses.Services.Abstractions;
using Courses.Services.Producers.CourseCreatedProducer;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using Task = System.Threading.Tasks.Task;

namespace Tests.Courses.UnitTests.ServiceTests;

public class CoursesServiceTests
{
    private readonly IMapper _mapper =
        new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new AppMappingProfile())));
    private readonly Mock<ICategoryRepository> _categoryRepository = new();
    private readonly Mock<ICourseRepository> _courseRepository = new();
    private readonly Mock<IBlockRepository> _blockRepository = new();
    private readonly Mock<IBlockCompletedInfoRepository> _blockCompletedInfoRepository = new();
    private readonly Mock<IUserRepository> _userRepository = new();
    private readonly Mock<IUserCourseInfoRepository> _userCourseInfoRepository = new();
    private readonly Mock<ICourseCreatedProducer> _courseCreatedProducer = new();
    private readonly Mock<ICourseManagementService> _courseManagementService = new();
    private readonly Mock<ISubscriptionManagementService> _subscriptionManagementService = new();
    private readonly CourseService _service;
    private readonly Fixture _fixture = new();

    public CoursesServiceTests()
    {
        _service = new CourseService(
            _courseRepository.Object,
            _userCourseInfoRepository.Object,
            _courseManagementService.Object,
            _blockCompletedInfoRepository.Object,
            _blockRepository.Object,
            _userRepository.Object,
            _courseCreatedProducer.Object,
            _categoryRepository.Object,
            _subscriptionManagementService.Object,
            _mapper);
    }

    [Fact]
    public async Task GetPreviewCourse_ReturnsNotFound_WhenCourseNotExist()
    {
        // Arrange
        var courseId = -1;

        _courseRepository.Setup(r => r.GetCourseAsync(-1))
            .Returns(Task.FromResult<Course?>(null));

        // Act
        var result = await _service.GetCoursePreview(courseId, Guid.NewGuid());

        // Assert
        Assert.IsType<NotFound>(result);
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
        var result = await _service.GetCoursePreview(courseId, Guid.NewGuid());

        // Assert
        var actionResult = Assert.IsType<Ok<PreviewResponse>>(result);
        var value = Assert.IsAssignableFrom<PreviewResponse>(
            actionResult.Value);
        Assert.Equal(courseId, value.Id);
        Assert.Equivalent(_mapper.Map<AuthorResponse>(course.Author), value.CourseAuthor, true);
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
        var result = await _service.GetCoursePreview(courseId, Guid.NewGuid());

        // Assert
        var actionResult = Assert.IsType<Ok<PreviewResponse>>(result);
        var value = Assert.IsAssignableFrom<PreviewResponse>(
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
        var result = await _service.GetCourseHierarchy(courseId, userId, null);

        // Assert
        Assert.IsType<NotFound>(result);
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
        _courseManagementService.Setup(x => x.AllowVisitCourse(It.IsAny<Guid>(), It.IsAny<int>(), null))
            .ReturnsAsync(true);

        // Act
        var result = await _service.GetCourseHierarchy(courseId, userId, null);

        // Assert
        var actionResult = Assert.IsType<Ok<CourseResponse>>(result);
        var value = Assert.IsAssignableFrom<CourseResponse>(
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
        var result = await _service.GetSummaryBlock(courseId, blockId, userId, null);

        // Assert
        Assert.IsType<ForbidHttpResult>(result);
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
        var result = await _service.GetSummaryBlock(courseId, blockId, userId, null);

        // Assert
        Assert.IsType<ForbidHttpResult>(result);
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
        _courseManagementService.Setup(x => x.AllowVisitCourse(It.IsAny<Guid>(), It.IsAny<int>(), null))
            .ReturnsAsync(true);

        // Act
        var result = await _service.GetSummaryBlock(courseId, blockId, userId, null);

        // Assert
        var actionResult = Assert.IsType<Ok<SummaryBlockResponse>>(result);
        var value = Assert.IsAssignableFrom<SummaryBlockResponse>(
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
        _courseManagementService.Setup(x => x.AllowVisitCourse(It.IsAny<Guid>(), It.IsAny<int>(), null))
            .ReturnsAsync(true);

        // Act
        var result = await _service.GetVideoBlock(courseId, blockId, userId, null);

        // Assert
        Assert.IsType<NotFound>(result);
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
        _courseManagementService.Setup(x => x.AllowVisitCourse(It.IsAny<Guid>(), It.IsAny<int>(), null))
            .ReturnsAsync(true);

        // Act
        var result = await _service.GetVideoBlock(courseId, blockId, userId, null);

        // Assert
        Assert.IsType<NotFound>(result);
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
        _courseManagementService.Setup(x => x.AllowVisitCourse(It.IsAny<Guid>(), It.IsAny<int>(), null))
            .ReturnsAsync(true);

        // Act
        var result = await _service.GetVideoBlock(courseId, blockId, userId, null);

        // Assert
        var actionResult = Assert.IsType<Ok<VideoBlockResponse>>(result);
        var value = Assert.IsAssignableFrom<VideoBlockResponse>(
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
        _courseManagementService.Setup(x => x.AllowVisitCourse(It.IsAny<Guid>(), It.IsAny<int>(), null))
            .ReturnsAsync(true);

        // Act
        var result = await _service.GetTaskBlock(courseId, blockId, userId, null);

        // Assert
        Assert.IsType<NotFound>(result);
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
        _courseManagementService.Setup(x => x.AllowVisitCourse(It.IsAny<Guid>(), It.IsAny<int>(), null))
            .ReturnsAsync(true);

        // Act
        var result = await _service.GetTaskBlock(courseId, blockId, userId, null);

        // Assert
        Assert.IsType<NotFound>(result);
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
        _courseManagementService.Setup(x => x.AllowVisitCourse(It.IsAny<Guid>(), It.IsAny<int>(), null))
            .ReturnsAsync(true);

        // Act
        var result = await _service.GetTaskBlock(courseId, blockId, userId, null);

        // Assert
        var actionResult = Assert.IsType<Ok<TasksBlockResponse>>(result);
        var value = Assert.IsAssignableFrom<TasksBlockResponse>(
            actionResult.Value);
        Assert.Equal(blockId, value.Id);
    }
}

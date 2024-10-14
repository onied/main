using AutoFixture;
using AutoMapper;
using Courses.Data.Models;
using Courses.Dtos.Catalog.Response;
using Courses.Dtos.Course.Response;
using Courses.Dtos.EditCourse.Request;
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
    private readonly Mock<ISubscriptionManagementService> _subscriptionManagementService = new();
    private readonly CourseService _service;
    private readonly Fixture _fixture = new();

    public CoursesServiceTests()
    {
        _service = new CourseService(
            _courseRepository.Object,
            _userCourseInfoRepository.Object,
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
            .ReturnsAsync(course);

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
            .ReturnsAsync(course);

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
        // Act
        var result = await _service.GetCourseHierarchy(-1, Guid.NewGuid(), null);

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
            .ReturnsAsync(course);
        _blockCompletedInfoRepository
            .Setup(r => r.GetAllCompletedCourseBlocksByUser(userId, courseId))
            .ReturnsAsync(new List<BlockCompletedInfo>());

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
        // Act
        var result = await _service.GetSummaryBlock(-1, -1, Guid.NewGuid(), null);

        // Assert
        Assert.IsType<NotFound>(result);
    }

    [Fact]
    public async Task GetSummaryBlock_ReturnsNotFound_WhenCourseNotRight()
    {
        // Arrange
        _blockRepository.Setup(x => x.GetSummaryBlock(It.IsAny<int>()))
            .ReturnsAsync(new SummaryBlock { Module = new Module { CourseId = 0 } });

        var courseId = -1;

        // Act
        var result = await _service.GetSummaryBlock(courseId, -1, Guid.NewGuid(), null);

        // Assert
        Assert.IsType<NotFound>(result);
    }

    [Fact]
    public async Task GetSummaryBlock_ReturnsBlock()
    {
        // Arrange
        var courseId = 1;
        var blockId = 1;

        _blockRepository.Setup(b => b.GetSummaryBlock(blockId))
            .ReturnsAsync(new SummaryBlock { Id = blockId, Module = new Module { CourseId = courseId } });

        // Act
        var result = await _service.GetSummaryBlock(courseId, blockId, Guid.NewGuid(), null);

        // Assert
        var actionResult = Assert.IsType<Ok<SummaryBlockResponse>>(result);
        var value = Assert.IsAssignableFrom<SummaryBlockResponse>(
            actionResult.Value);

        Assert.Equal(blockId, value.Id);
    }

    [Fact]
    public async Task GetVideoBlock_ReturnsNotFound_WhenBlockNotExist()
    {
        // Act
        var result = await _service.GetVideoBlock(-1, -1, Guid.NewGuid(), null);

        // Assert
        Assert.IsType<NotFound>(result);
    }

    [Fact]
    public async Task GetVideoBlock_ReturnsNotFound_WhenCourseNotRight()
    {
        // Arrange
        _blockRepository.Setup(x => x.GetVideoBlock(It.IsAny<int>()))
            .ReturnsAsync(new VideoBlock { Module = new Module { CourseId = 0 } });

        var courseId = -1;

        // Act
        var result = await _service.GetVideoBlock(courseId, -1, Guid.NewGuid(), null);

        // Assert
        Assert.IsType<NotFound>(result);
    }

    [Fact]
    public async Task GetVideoBlock_ReturnsBlock()
    {
        // Arrange
        var courseId = 1;
        var blockId = 1;

        _blockRepository.Setup(b => b.GetVideoBlock(blockId))
            .ReturnsAsync(new VideoBlock { Id = blockId, Module = new Module { CourseId = courseId } });

        // Act
        var result = await _service.GetVideoBlock(courseId, blockId, Guid.NewGuid(), null);

        // Assert
        var actionResult = Assert.IsType<Ok<VideoBlockResponse>>(result);
        var value = Assert.IsAssignableFrom<VideoBlockResponse>(
            actionResult.Value);
        Assert.Equal(blockId, value.Id);
    }

    [Fact]
    public async Task GetTasksBlock_ReturnsNotFound_WhenBlockNotExist()
    {
        // Act
        var result = await _service.GetTaskBlock(-1, -1, Guid.NewGuid(), null);

        // Assert
        Assert.IsType<NotFound>(result);
    }

    [Fact]
    public async Task GetTasksBlock_ReturnsNotFound_WhenCourseNotRight()
    {
        // Arrange
        _blockRepository.Setup(x => x.GetTasksBlock(It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<bool>()))
            .ReturnsAsync(new TasksBlock { Module = new Module { CourseId = 0 } });

        var courseId = -1;

        // Act
        var result = await _service.GetTaskBlock(courseId, -1, Guid.NewGuid(), null);

        // Assert
        Assert.IsType<NotFound>(result);
    }

    [Fact]
    public async Task GetTasksBlock_ReturnsBlock()
    {
        // Arrange
        var courseId = 1;
        var blockId = 1;

        _blockRepository.Setup(b => b.GetTasksBlock(blockId, It.IsAny<bool>(), It.IsAny<bool>()))
            .ReturnsAsync(new TasksBlock { Id = blockId, Module = new Module { CourseId = courseId } });

        // Act
        var result = await _service.GetTaskBlock(courseId, blockId, Guid.NewGuid(), null);

        // Assert
        var actionResult = Assert.IsType<Ok<TasksBlockResponse>>(result);
        var value = Assert.IsAssignableFrom<TasksBlockResponse>(
            actionResult.Value);
        Assert.Equal(blockId, value.Id);
    }

    [Fact]
    public async Task CreateCourse_ShouldReturnUnauthorized_WhenUserIdIsNull()
    {
        // Act
        var result = await _service.CreateCourse(null);

        // Assert
        Assert.IsType<UnauthorizedHttpResult>(result);
    }

    [Fact]
    public async Task CreateCourse_ShouldReturnUnauthorized_WhenUserDoesNotExist()
    {
        // Act
        var result = await _service.CreateCourse(Guid.NewGuid().ToString());

        // Assert
        Assert.IsType<UnauthorizedHttpResult>(result);
    }

    [Fact]
    public async Task CreateCourse_ShouldReturnForbid_WhenUserCannotCreateCourses()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var authorId = Guid.Parse(userId);
        var user = _fixture.Create<User>();

        _userRepository.Setup(repo => repo.GetUserAsync(authorId))
            .ReturnsAsync(user);
        _subscriptionManagementService.Setup(service => service.VerifyCreatingCoursesAsync(authorId))
            .ReturnsAsync(false);

        // Act
        var result = await _service.CreateCourse(userId);

        // Assert
        Assert.IsType<ForbidHttpResult>(result);
    }

    [Fact]
    public async Task CreateCourse_ShouldCreateCourseAndReturnOk_WhenSuccessful()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var authorId = Guid.Parse(userId);
        var user = _fixture.Create<User>();
        var category = _fixture.Create<Category>();

        _userRepository.Setup(repo => repo.GetUserAsync(authorId))
            .ReturnsAsync(user);
        _subscriptionManagementService.Setup(service => service.VerifyCreatingCoursesAsync(authorId))
            .ReturnsAsync(true);
        _categoryRepository.Setup(repo => repo.GetAllCategoriesAsync())
            .ReturnsAsync([category]);

        var newCourse = _fixture.Create<Course>();
        _courseRepository.Setup(repo => repo.AddCourseAsync(It.IsAny<Course>()))
            .ReturnsAsync(newCourse);

        // Act
        var result = await _service.CreateCourse(userId);

        // Assert
        var actionResult = Assert.IsType<Ok<CreateCourseResponse>>(result);
        var value = Assert.IsType<CreateCourseResponse>(actionResult.Value);
        Assert.Equal(newCourse.Id, value.Id);

        // Ensure that PublishAsync was called
        _courseCreatedProducer.Verify(producer => producer.PublishAsync(newCourse), Times.Once);
    }

    [Fact]
    public async Task GetEditTaskBlock_ReturnsNotFound_WhenBlockNotExist()
    {
        // Act
        var result = await _service.GetEditTaskBlock(-1, -1, Guid.NewGuid(), null);

        // Assert
        Assert.IsType<NotFound>(result);
    }

    [Fact]
    public async Task GetEditTaskBlock_ReturnsNotFound_WhenCourseNotRight()
    {
        // Arrange
        _blockRepository.Setup(x => x.GetTasksBlock(It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<bool>()))
            .ReturnsAsync(new TasksBlock { Module = new Module { CourseId = 0 } });

        var courseId = -1;

        // Act
        var result = await _service.GetTaskBlock(courseId, -1, Guid.NewGuid(), null);

        // Assert
        Assert.IsType<NotFound>(result);
    }

    [Fact]
    public async Task GetEditTaskBlock_ReturnsBlock()
    {
        // Arrange
        var courseId = 1;
        var blockId = 1;

        _blockRepository.Setup(b => b.GetTasksBlock(blockId, It.IsAny<bool>(), It.IsAny<bool>()))
            .ReturnsAsync(new TasksBlock { Id = blockId, Module = new Module { CourseId = courseId } });

        // Act
        var result = await _service.GetEditTaskBlock(courseId, blockId, Guid.NewGuid(), "User");

        // Assert
        var actionResult = Assert.IsType<Ok<EditTasksBlockRequest>>(result);
        var value = Assert.IsType<EditTasksBlockRequest>(actionResult.Value);
        Assert.Equal(blockId, value.Id);
    }

    [Fact]
    public async Task EnterFreeCourse_ShouldReturnNotFound_WhenCourseIsNull()
    {
        // Act
        var result = await _service.EnterFreeCourse(-1, Guid.NewGuid());

        // Assert
        Assert.IsType<NotFound>(result);
    }

    [Fact]
    public async Task EnterFreeCourse_ShouldReturnNotFound_WhenCourseIsNotFree()
    {
        // Arrange
        var id = 1;
        var userId = Guid.NewGuid();
        var course = new Course { Id = id, PriceRubles = 100 };

        _courseRepository.Setup(repo => repo.GetCourseAsync(id))
            .ReturnsAsync(course);

        // Act
        var result = await _service.EnterFreeCourse(id, userId);

        // Assert
        Assert.IsType<NotFound>(result);
    }

    [Fact]
    public async Task EnterFreeCourse_ShouldReturnForbid_WhenUserAlreadyEnteredCourse()
    {
        // Arrange
        var id = 1;
        var userId = Guid.NewGuid();
        var course = new Course { Id = id, PriceRubles = 0 };

        _courseRepository.Setup(repo => repo.GetCourseAsync(id))
            .ReturnsAsync(course);
        _userCourseInfoRepository.Setup(repo => repo.GetUserCourseInfoAsync(userId, course.Id, false))
            .ReturnsAsync(new UserCourseInfo());

        // Act
        var result = await _service.EnterFreeCourse(id, userId);

        // Assert
        Assert.IsType<ForbidHttpResult>(result);
    }

    [Fact]
    public async Task EnterFreeCourse_ShouldAddUserCourseInfoAndReturnOk_WhenSuccessful()
    {
        // Arrange
        var id = 1;
        var userId = Guid.NewGuid();
        var course = new Course { Id = id, PriceRubles = 0 };

        _courseRepository.Setup(repo => repo.GetCourseAsync(id))
            .ReturnsAsync(course);

        // Act
        var result = await _service.EnterFreeCourse(id, userId);

        // Assert
        Assert.IsType<Ok>(result);

        // Verify that UserCourseInfo was added
        _userCourseInfoRepository.Verify(repo =>
            repo.AddUserCourseInfoAsync(It.Is<UserCourseInfo>(info => info.UserId == userId && info.CourseId == course.Id)), Times.Once);
    }
}

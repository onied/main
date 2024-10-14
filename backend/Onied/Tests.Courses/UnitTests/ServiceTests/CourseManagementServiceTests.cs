using AutoFixture;
using AutoMapper;
using Courses.Data.Models;
using Courses.Dtos.Course.Response;
using Courses.Dtos.EditCourse.Request;
using Courses.Dtos.Moderator.Response;
using Courses.Profiles;
using Courses.Services;
using Courses.Services.Abstractions;
using Courses.Services.Producers.CourseUpdatedProducer;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using Task = System.Threading.Tasks.Task;

namespace Tests.Courses.UnitTests.ServiceTests;

public class CourseManagementServiceTests
{
    private readonly IMapper _mapper =
        new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new AppMappingProfile())));
    private readonly Mock<ICourseRepository> _courseRepository = new();
    private readonly Mock<IUserCourseInfoRepository> _userCourseInfoRepository = new();
    private readonly Mock<IHttpClientFactory> _httpClientFactory = new();
    private readonly Mock<IBlockRepository> _blockRepository = new();
    private readonly Mock<ICategoryRepository> _categoryRepository = new();
    private readonly Mock<IModuleRepository> _moduleRepository = new();
    private readonly Mock<IUpdateTasksBlockService> _updateTasksBlockService = new();
    private readonly Mock<ICourseUpdatedProducer> _courseUpdatedProducer = new();
    private readonly Mock<ISubscriptionManagementService> _subscriptionManagementService = new();
    private readonly CourseManagementService _service;
    private readonly Fixture _fixture = new();

    public CourseManagementServiceTests()
    {
        _service = new CourseManagementService(
            _courseRepository.Object,
            _userCourseInfoRepository.Object,
            _httpClientFactory.Object,
            _blockRepository.Object,
            _updateTasksBlockService.Object,
            _moduleRepository.Object,
            _categoryRepository.Object,
            _courseUpdatedProducer.Object,
            _subscriptionManagementService.Object,
            _mapper);
    }

    [Fact]
    public async Task EditCourse_ReturnsNotFound_WhenCourseNotExist()
    {
        // Arrange
        var courseId = -1;
        var userId = Guid.NewGuid().ToString();
        var editCourseDto = _fixture.Build<EditCourseRequest>().Create();

        // Act
        var result = await _service.EditCourse(courseId, editCourseDto, userId);

        // Assert
        Assert.IsType<NotFound>(result);
    }

    [Fact]
    public async Task EditCourse_ReturnsBadRequest_WhenCategoryDoesNotExist()
    {
        // Arrange
        var user = _fixture.Build<User>().Create();
        var course = new Course();
        var editCourseDto = _mapper.Map<EditCourseRequest>(course);

        _courseRepository.Setup(r => r.GetCourseAsync(course.Id))
            .ReturnsAsync(course);
        editCourseDto.CategoryId = 0;

        // Act
        var result = await _service.EditCourse(course.Id, editCourseDto, user.Id.ToString());

        // Assert
        Assert.IsType<ProblemHttpResult>(result);
    }

    [Fact]
    public async Task EditCourse_ReturnsCoursePreview_WhenNothingChanged()
    {
        // Arrange
        var course = new Course();
        var category = new Category();
        var editCourseDto = _mapper.Map<EditCourseRequest>(course);

        _courseRepository.Setup(r => r.GetCourseAsync(course.Id))
            .ReturnsAsync(course);
        _categoryRepository.Setup(r => r.GetCategoryById(category.Id))
            .ReturnsAsync(category);
        _subscriptionManagementService.Setup(x => x.VerifyGivingCertificatesAsync(It.IsAny<Guid>()))
            .ReturnsAsync(true);

        // Act
        var result = await _service.EditCourse(course.Id, editCourseDto, Guid.NewGuid().ToString());

        // Assert
        Assert.IsType<Ok<PreviewResponse>>(result);
        var actualResult = (result as Ok<PreviewResponse>)?.Value!;
        Assert.Equal(0, actualResult.Id);
    }

    [Fact]
    public async Task EditCourse_ReturnsChangedCoursePreview_WhenEverythingChanged()
    {
        // Arrange
        var user = _fixture.Build<User>().Create();
        var category = _fixture.Build<Category>().Create();
        var course = _fixture.Build<Course>()
            .With(course => course.Author, user)
            .With(course => course.AuthorId, user.Id).Create();
        var editCourseDto = new EditCourseRequest
        {
            CategoryId = category.Id,
            Description = "Trollface",
            HasCertificates = !course.HasCertificates,
            HoursCount = 89,
            IsArchived = !course.IsArchived,
            IsProgramVisible = !course.IsProgramVisible,
            PictureHref = "https://upload.wikimedia.org/wikipedia/en/9/9a/Trollface_non-free.png",
            Price = 1234,
            Title = "Trollface"
        };
        var editedCourse = _mapper.Map(editCourseDto, course);
        editedCourse.Category = category;
        editedCourse.CategoryId = category.Id;
        var coursePreview = _mapper.Map<PreviewResponse>(editedCourse);

        _courseRepository.Setup(r => r.GetCourseAsync(course.Id))
            .Returns(Task.FromResult<Course?>(course));
        _categoryRepository.Setup(r => r.GetCategoryById(category.Id))
            .Returns(Task.FromResult<Category?>(category));

        // Act
        var result = await _service.EditCourse(course.Id, editCourseDto, user.Id.ToString());

        // Assert
        Assert.IsType<Ok<PreviewResponse>>(result);
        var actualResult = (result as Ok<PreviewResponse>)?.Value;
        Assert.Equivalent(coursePreview, actualResult);
    }

    [Fact]
    public async Task CheckCourseAuthor_ReturnsNotFound_WhenCourseDoesNotExist()
    {
        // Arrange
        var courseId = -1;
        var userId = Guid.NewGuid().ToString();

        // Act
        var result = await _service.CheckCourseAuthorAsync(courseId, userId, null);

        // Assert
        Assert.IsType<NotFound>(result);
    }

    [Fact]
    public async Task CheckCourseAuthor_ReturnsForbidden_WhenUserIsNotAuthor()
    {
        // Arrange
        var course = new Course { AuthorId = Guid.NewGuid() };
        var courseId = course.Id;
        var userId = Guid.NewGuid().ToString();

        _courseRepository.Setup(r => r.GetCourseAsync(courseId))
            .ReturnsAsync(course);

        // Act
        var result = await _service.CheckCourseAuthorAsync(courseId, userId, null);

        // Assert
        Assert.IsType<ForbidHttpResult>(result);
    }

    [Fact]
    public async Task CheckCourseAuthor_ReturnsOk()
    {
        // Arrange
        var user = new User { Id = Guid.NewGuid() };
        var course = new Course { Author = user };
        var courseId = course.Id;

        _courseRepository.Setup(r => r.GetCourseAsync(courseId))
            .ReturnsAsync(course);

        // Act
        var result = await _service.CheckCourseAuthorAsync(courseId, user.Id.ToString(), null);

        // Assert
        Assert.IsType<Ok<string>>(result);
    }

    [Fact]
    public async Task AllowVisitCourse_ReturnsTrue_WhenUserHasFreeCourse()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var courseId = 1;
        var userCourseInfo = new UserCourseInfo
        {
            Course = new Course { PriceRubles = 0 },
            Token = "sample-token"
        };

        _userCourseInfoRepository.Setup(r => r.GetUserCourseInfoAsync(userId, courseId, true))
            .ReturnsAsync(userCourseInfo);

        // Act
        var result = await _service.AllowVisitCourse(userId, courseId);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task AllowVisitCourse_ReturnsFalse_WhenUserCourseInfoIsNull()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var courseId = 1;

        _userCourseInfoRepository.Setup(r => r.GetUserCourseInfoAsync(userId, courseId, true))
            .ReturnsAsync((UserCourseInfo?)null);

        // Act
        var result = await _service.AllowVisitCourse(userId, courseId);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task GetStudents_ReturnsNotFound_WhenCourseDoesNotExist()
    {
        // Arrange
        var courseId = -1;
        var authorId = Guid.NewGuid();

        // Act
        var result = await _service.GetStudents(courseId, authorId);

        // Assert
        Assert.IsType<NotFound>(result);
    }

    [Fact]
    public async Task GetStudents_ReturnsForbid_WhenUserIsNotAuthor()
    {
        // Arrange
        var courseId = 1;

        _courseRepository.Setup(x => x.GetCourseWithUsersAndModeratorsAsync(It.IsAny<int>()))
            .ReturnsAsync(new Course());

        // Act
        var result = await _service.GetStudents(courseId, Guid.NewGuid());

        // Assert
        Assert.IsType<ForbidHttpResult>(result);
    }

    [Fact]
    public async Task GetStudents_ReturnsOk()
    {
        // Arrange
        var courseId = 1;
        var author = new User { Id = Guid.NewGuid() };

        _courseRepository.Setup(x => x.GetCourseWithUsersAndModeratorsAsync(It.IsAny<int>()))
            .ReturnsAsync(new Course { Author = author });

        // Act
        var result = await _service.GetStudents(courseId, author.Id);

        // Assert
        Assert.IsType<Ok<CourseStudentsResponse>>(result);
    }

    [Fact]
    public async Task AddModerator_ReturnsNotFound_WhenCourseDoesNotExist()
    {
        // Arrange
        var courseId = -1;
        var studentId = Guid.NewGuid();
        var authorId = Guid.NewGuid();

        _courseRepository.Setup(r => r.GetCourseWithUsersAndModeratorsAsync(courseId))
            .ReturnsAsync((Course?)null);

        // Act
        var result = await _service.AddModerator(courseId, studentId, authorId);

        // Assert
        Assert.IsType<NotFound<string>>(result);
    }

    [Fact]
    public async Task AddModerator_ReturnsForbid_WhenUserIsNotAuthor()
    {
        // Arrange
        var courseId = -1;

        _courseRepository.Setup(x => x.GetCourseWithUsersAndModeratorsAsync(It.IsAny<int>()))
            .ReturnsAsync(new Course());

        // Act
        var result = await _service.AddModerator(courseId, Guid.NewGuid(), Guid.NewGuid());

        // Assert
        Assert.IsType<ForbidHttpResult>(result);
    }

    [Fact]
    public async Task AddModerator_ReturnsNotFound_WhenUserIsNotStudent()
    {
        // Arrange
        var courseId = 1;
        var author = new User { Id = Guid.NewGuid() };

        _courseRepository.Setup(x => x.GetCourseWithUsersAndModeratorsAsync(It.IsAny<int>()))
            .ReturnsAsync(new Course { Author = author });

        // Act
        var result = await _service.AddModerator(courseId, Guid.NewGuid(), author.Id);

        // Assert
        Assert.IsType<NotFound<string>>(result);
    }

    [Fact]
    public async Task AddModerator_ReturnsOk()
    {
        // Arrange
        var courseId = 1;
        var author = new User { Id = Guid.NewGuid() };
        var studentId = Guid.NewGuid();

        _courseRepository.Setup(x => x.GetCourseWithUsersAndModeratorsAsync(It.IsAny<int>()))
            .ReturnsAsync(new Course { Author = author, Users = { new User { Id = studentId } } });

        // Act
        var result = await _service.AddModerator(courseId, studentId, author.Id);

        // Assert
        Assert.IsType<Ok>(result);
    }

    [Fact]
    public async Task DeleteModerator_ReturnsNotFound_WhenCourseDoesNotExist()
    {
        // Arrange
        var courseId = -1;
        var studentId = Guid.NewGuid();
        var authorId = Guid.NewGuid();

        // Act
        var result = await _service.AddModerator(courseId, studentId, authorId);

        // Assert
        Assert.IsType<NotFound<string>>(result);
    }

    [Fact]
    public async Task DeleteModerator_ReturnsForbid_WhenUserIsNotAuthor()
    {
        // Arrange
        var courseId = -1;

        _courseRepository.Setup(x => x.GetCourseWithUsersAndModeratorsAsync(It.IsAny<int>()))
            .ReturnsAsync(new Course());

        // Act
        var result = await _service.AddModerator(courseId, Guid.NewGuid(), Guid.NewGuid());

        // Assert
        Assert.IsType<ForbidHttpResult>(result);
    }

    [Fact]
    public async Task DeleteModerator_ReturnsNotFound_WhenUserIsNotStudent()
    {
        // Arrange
        var courseId = 1;
        var author = new User { Id = Guid.NewGuid() };

        _courseRepository.Setup(x => x.GetCourseWithUsersAndModeratorsAsync(It.IsAny<int>()))
            .ReturnsAsync(new Course { Author = author });

        // Act
        var result = await _service.DeleteModerator(courseId, Guid.NewGuid(), author.Id);

        // Assert
        Assert.IsType<NotFound<string>>(result);
    }

    [Fact]
    public async Task DeleteModerator_ReturnsOk()
    {
        // Arrange
        var author = new User { Id = Guid.NewGuid() };
        var studentId = Guid.NewGuid();
        var course = new Course { Author = author };
        course.Moderators.Add(new User { Id = studentId });

        _courseRepository.Setup(x => x.GetCourseWithUsersAndModeratorsAsync(It.IsAny<int>()))
            .ReturnsAsync(course);

        // Act
        var result = await _service.DeleteModerator(course.Id, studentId, author.Id);

        // Assert
        Assert.IsType<Ok>(result);
    }

    [Fact]
    public async Task EditTasksBlock_ReturnsNotFound_WhenBlockDoesNotExist()
    {
        // Arrange
        var courseId = 1;
        var blockId = -1;
        var tasksBlockRequest = new EditTasksBlockRequest();

        // Act
        var result = await _service.EditTasksBlock(courseId, blockId, tasksBlockRequest);

        // Assert
        Assert.IsType<NotFound>(result);
    }

    [Fact]
    public async Task EditTasksBlock_ReturnsOk()
    {
        // Arrange
        var courseId = 1;
        var blockId = 1;
        var tasksBlockRequest = new EditTasksBlockRequest();

        _blockRepository.Setup(x => x.GetTasksBlock(It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<bool>()))
            .ReturnsAsync(new TasksBlock { Module = new Module { CourseId = courseId } });
        _updateTasksBlockService.Setup(x => x.UpdateTasksBlock(It.IsAny<EditTasksBlockRequest>()))
            .ReturnsAsync(new TasksBlock());

        // Act
        var result = await _service.EditTasksBlock(courseId, blockId, tasksBlockRequest);

        // Assert
        Assert.IsType<Ok<EditTasksBlockRequest>>(result);
    }

    [Fact]
    public async Task EditSummaryBlock_ReturnsNotFound_WhenBlockDoesNotExist()
    {
        // Arrange
        var courseId = 1;
        var blockId = -1;
        var summaryBlockResponse = new SummaryBlockResponse();

        // Act
        var result = await _service.EditSummaryBlock(courseId, blockId, summaryBlockResponse);

        // Assert
        Assert.IsType<NotFound>(result);
    }

    [Fact]
    public async Task EditSummaryBlock_ReturnsOk()
    {
        // Arrange
        var courseId = 1;
        var blockId = 1;
        var summaryBlockResponse = new SummaryBlockResponse();

        _blockRepository.Setup(x => x.GetSummaryBlock(It.IsAny<int>()))
            .ReturnsAsync(new SummaryBlock { Module = new Module { CourseId = courseId } });

        // Act
        var result = await _service.EditSummaryBlock(courseId, blockId, summaryBlockResponse);

        // Assert
        Assert.IsType<Ok>(result);
    }

    [Fact]
    public async Task EditVideoBlock_ReturnsNotFound_WhenBlockDoesNotExist()
    {
        // Arrange
        var courseId = 1;
        var blockId = -1;
        var videoBlockResponse = new VideoBlockResponse();

        // Act
        var result = await _service.EditVideoBlock(courseId, blockId, videoBlockResponse);

        // Assert
        Assert.IsType<NotFound>(result);
    }

    [Fact]
    public async Task EditVideoBlock_ReturnsOk()
    {
        // Arrange
        var courseId = 1;
        var blockId = 1;
        var videoBlockResponse = new VideoBlockResponse();

        _blockRepository.Setup(x => x.GetVideoBlock(It.IsAny<int>()))
            .ReturnsAsync(new VideoBlock { Module = new Module { CourseId = courseId } });

        // Act
        var result = await _service.EditVideoBlock(courseId, blockId, videoBlockResponse);

        // Assert
        Assert.IsType<Ok>(result);
    }

    [Fact]
    public async Task DeleteBlock_ReturnsNotFound_WhenBlockDoesNotExist()
    {
        // Arrange
        var courseId = 1;
        var blockId = -1;

        _blockRepository.Setup(r => r.DeleteBlockAsync(blockId))
            .ReturnsAsync(false);

        // Act
        var result = await _service.DeleteBlock(courseId, blockId);

        // Assert
        Assert.IsType<NotFound>(result);
    }

    [Fact]
    public async Task AddBlock_ReturnsNotFound_WhenModuleDoesNotExist()
    {
        // Arrange
        var courseId = 1;
        var moduleId = -1;
        var blockType = (int)BlockType.VideoBlock; // Example block type

        _moduleRepository.Setup(r => r.GetModuleAsync(moduleId))
            .ReturnsAsync((Module?)null);

        // Act
        var result = await _service.AddBlock(courseId, moduleId, blockType);

        // Assert
        Assert.IsType<NotFound>(result);
    }

    [Fact]
    public async Task AddBlock_ReturnsOk()
    {
        // Arrange
        var courseId = 1;
        var moduleId = 1;
        var blockType = (int)BlockType.VideoBlock;

        _moduleRepository.Setup(r => r.GetModuleAsync(moduleId))
            .ReturnsAsync(new Module());

        // Act
        var result = await _service.AddBlock(courseId, moduleId, blockType);

        // Assert
        Assert.IsType<Ok<int>>(result);
    }

    [Fact]
    public async Task RenameModule_ReturnsNotFound_WhenModuleDoesNotExist()
    {
        // Arrange
        var courseId = 1;
        var renameModuleRequest = new RenameModuleRequest { ModuleId = -1 };

        _moduleRepository.Setup(r => r.RenameModuleAsync(renameModuleRequest.ModuleId, It.IsAny<string>()))
            .ReturnsAsync(false);

        // Act
        var result = await _service.RenameModule(courseId, renameModuleRequest);

        // Assert
        Assert.IsType<NotFound>(result);
    }

    [Fact]
    public async Task DeleteModule_ReturnsNotFound_WhenModuleDoesNotExist()
    {
        // Arrange
        var courseId = 1;
        var moduleId = -1;

        _moduleRepository.Setup(r => r.DeleteModuleAsync(moduleId))
            .ReturnsAsync(false);

        // Act
        var result = await _service.DeleteModule(courseId, moduleId);

        // Assert
        Assert.IsType<NotFound>(result);
    }

    [Fact]
    public async Task EditHierarchy_ReturnsNotFound_WhenCourseDoesNotExist()
    {
        // Arrange
        var courseId = -1;
        var courseResponse = new CourseResponse();

        _courseRepository.Setup(r => r.GetCourseAsync(courseId))
            .ReturnsAsync((Course?)null);

        // Act
        var result = await _service.EditHierarchy(courseId, courseResponse);

        // Assert
        Assert.IsType<NotFound>(result);
    }
}

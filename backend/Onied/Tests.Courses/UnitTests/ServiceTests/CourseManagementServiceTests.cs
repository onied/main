using AutoFixture;
using AutoMapper;
using Courses.Data.Models;
using Courses.Dtos.Course.Response;
using Courses.Dtos.EditCourse.Request;
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

        _courseRepository.Setup(r => r.GetCourseAsync(-1))
            .Returns(Task.FromResult<Course?>(null));

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
        var course = _fixture.Build<Course>()
            .With(course => course.Author, user)
            .With(course => course.AuthorId, user.Id).Create();
        var editCourseDto = _mapper.Map<EditCourseRequest>(course);

        _courseRepository.Setup(r => r.GetCourseAsync(course.Id))
            .Returns(Task.FromResult<Course?>(course));
        _categoryRepository.Setup(r => r.GetCategoryById(0))
            .Returns(Task.FromResult<Category?>(null));
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
        var user = _fixture.Build<User>().Create();
        var category = _fixture.Build<Category>().Create();
        var course = _fixture.Build<Course>()
            .With(course => course.Author, user)
            .With(course => course.AuthorId, user.Id)
            .With(course => course.Category, category)
            .With(course => course.CategoryId, category.Id).Create();
        var editCourseDto = _mapper.Map<EditCourseRequest>(course);
        var coursePreview = _mapper.Map<PreviewResponse>(course);

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
}

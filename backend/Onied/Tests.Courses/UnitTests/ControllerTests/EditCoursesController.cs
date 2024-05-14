using AutoFixture;
using AutoMapper;
using Courses.Controllers;
using Courses.Dtos;
using Courses.Models;
using Courses.Profiles;
using Courses.Services;
using Courses.Services.Abstractions;
using Courses.Services.Producers.CourseUpdatedProducer;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;
using Moq;
using Task = System.Threading.Tasks.Task;

namespace Tests.Courses.UnitTests.ControllerTests;

public class EditCoursesControllerTests
{
    private readonly Mock<ILogger<CoursesController>> _logger = new();
    private readonly IMapper _mapper =
        new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new AppMappingProfile())));
    private readonly Mock<ICourseRepository> _courseRepository = new();
    private readonly Mock<IBlockRepository> _blockRepository = new();
    private readonly Mock<ICourseManagementService> _courseManagementService = new();
    private readonly Mock<ICategoryRepository> _categoryRepository = new();
    private readonly Mock<IModuleRepository> _moduleRepository = new();
    private readonly Mock<UpdateTasksBlockService> _updateTasksBlockService = new();
    private readonly Mock<ISubscriptionManagementService> _subscriptionManagementService = new();
    private readonly Mock<CourseUpdatedProducer> _courseUpdatedProducer = new();
    private readonly EditCoursesController _controller;
    private readonly Fixture _fixture = new();

    public EditCoursesControllerTests()
    {
        _controller = new EditCoursesController(
            _logger.Object,
            _mapper,
            _courseRepository.Object,
            _blockRepository.Object,
            _courseManagementService.Object,
            _categoryRepository.Object,
            _moduleRepository.Object,
            _updateTasksBlockService.Object,
            _subscriptionManagementService.Object,
            _courseUpdatedProducer.Object);
    }

    [Fact]
    public async Task EditCourse_ReturnsNotFound_WhenCourseNotExist()
    {
        // Arrange
        var courseId = -1;
        var userId = Guid.NewGuid().ToString();
        var editCourseDto = _fixture.Build<EditCourseDto>().Create();

        _courseRepository.Setup(r => r.GetCourseAsync(-1))
            .Returns(Task.FromResult<Course?>(null));

        // Act
        var result = await _controller.EditCourse(courseId, userId, null, editCourseDto);

        // Assert
        Assert.IsType<NotFound>(result.Result);
    }

    [Fact]
    public async Task EditCourse_ReturnsUnauthorized_WhenUserNotAuthor()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();

        var course = _fixture.Build<Course>().Create();
        var editCourseDto = _mapper.Map<EditCourseDto>(course);
        var courseId = course.Id;

        _courseRepository.Setup(r => r.GetCourseAsync(courseId))
            .Returns(Task.FromResult<Course?>(course));

        // Act
        var result = await _controller.EditCourse(courseId, userId, null, editCourseDto);

        // Assert
        Assert.IsType<UnauthorizedHttpResult>(result.Result);
    }

    [Fact]
    public async Task EditCourse_ReturnsBadRequest_WhenCategoryDoesNotExist()
    {
        // Arrange
        var user = _fixture.Build<User>().Create();
        var course = _fixture.Build<Course>()
            .With(course => course.Author, user)
            .With(course => course.AuthorId, user.Id).Create();
        var editCourseDto = _mapper.Map<EditCourseDto>(course);

        _courseRepository.Setup(r => r.GetCourseAsync(course.Id))
            .Returns(Task.FromResult<Course?>(course));
        _categoryRepository.Setup(r => r.GetCategoryById(0))
            .Returns(Task.FromResult<Category?>(null));
        editCourseDto.CategoryId = 0;

        // Act
        var result = await _controller.EditCourse(course.Id, user.Id.ToString(), null, editCourseDto);

        // Assert
        Assert.IsType<ValidationProblem>(result.Result);
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
        var editCourseDto = _mapper.Map<EditCourseDto>(course);
        var coursePreview = _mapper.Map<PreviewDto>(course);

        _courseRepository.Setup(r => r.GetCourseAsync(course.Id))
            .Returns(Task.FromResult<Course?>(course));
        _categoryRepository.Setup(r => r.GetCategoryById(category.Id))
            .Returns(Task.FromResult<Category?>(category));

        // Act
        var result = await _controller.EditCourse(course.Id, user.Id.ToString(), null, editCourseDto);

        // Assert
        Assert.IsType<Ok<PreviewDto>>(result.Result);
        var actualResult = (result.Result as Ok<PreviewDto>)?.Value;
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
        var editCourseDto = new EditCourseDto
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
        var coursePreview = _mapper.Map<PreviewDto>(editedCourse);

        _courseRepository.Setup(r => r.GetCourseAsync(course.Id))
            .Returns(Task.FromResult<Course?>(course));
        _categoryRepository.Setup(r => r.GetCategoryById(category.Id))
            .Returns(Task.FromResult<Category?>(category));

        // Act
        var result = await _controller.EditCourse(course.Id, user.Id.ToString(), null, editCourseDto);

        // Assert
        Assert.IsType<Ok<PreviewDto>>(result.Result);
        var actualResult = (result.Result as Ok<PreviewDto>)?.Value;
        Assert.Equivalent(coursePreview, actualResult);
    }
}

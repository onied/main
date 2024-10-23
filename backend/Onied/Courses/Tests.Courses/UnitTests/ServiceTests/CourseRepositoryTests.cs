using AutoFixture;
using Courses.Data;
using Courses.Data.Models;
using Courses.Dtos.Catalog.Request;
using Courses.Services;
using Courses.Services.Abstractions;
using Tests.Courses.Helpers;
using Task = System.Threading.Tasks.Task;

namespace Tests.Courses.UnitTests.ServiceTests;

public class CourseRepositoryTests
{
    private readonly Fixture _fixture = new();
    private readonly AppDbContext _context;
    private readonly ICourseRepository _courseRepository;
    private readonly Guid _existingUserId = Guid.NewGuid();

    private int _existingCourseId;
    private int _notExistingCourseId;
    private Course _course = null!;


    public CourseRepositoryTests()
    {
        _context = AppDbContextTest.GetContext();
        _courseRepository = new CourseRepository(_context);
        ProduceTestData();
    }

    [Fact]
    public async Task CountAsync_ReturnsCorrectCount()
    {
        // Act
        var count = await _courseRepository.CountAsync();

        // Assert
        Assert.Equal(2, count);
    }

    [Fact]
    public async Task GetCoursesAsync_ReturnsCourses()
    {
        // Act
        var courses = await _courseRepository.GetCoursesAsync();

        // Assert
        Assert.NotEmpty(courses);
    }

    [Fact]
    public async Task GetCourseAsync_ExistingId_ReturnsCourse()
    {
        // Act
        var course = await _courseRepository.GetCourseAsync(_existingCourseId);

        // Assert
        Assert.NotNull(course);
        Assert.Equal(_existingCourseId, course.Id);
    }

    [Fact]
    public async Task GetCourseAsync_NonExistingId_ReturnsNull()
    {
        // Act
        var course = await _courseRepository.GetCourseAsync(_notExistingCourseId);

        // Assert
        Assert.Null(course);
    }

    [Fact]
    public async Task AddCourseAsync_CourseStored()
    {
        // Arrange
        var newCourse = _fixture.Build<Course>()
            .With(c => c.Id, 0)
            .Create();

        // Act
        var addedCourse = await _courseRepository.AddCourseAsync(newCourse);

        // Assert
        Assert.NotNull(addedCourse);
        Assert.NotEqual(0, addedCourse.Id);
        var actualCourse = await _context.Courses.FindAsync(addedCourse.Id);
        Assert.NotNull(actualCourse);
        Assert.Equal(newCourse.Title, actualCourse.Title);
    }

    [Fact]
    public async Task DeleteModeratorAsync_ModeratorRemoved()
    {
        // Arrange
        await _courseRepository.AddModeratorAsync(_existingCourseId, _existingUserId);

        // Act
        await _courseRepository.DeleteModeratorAsync(_existingCourseId, _existingUserId);
        var course = await _courseRepository.GetCourseWithModeratorsAsync(_existingCourseId);

        // Assert
        Assert.NotNull(course);
        Assert.DoesNotContain(course.Moderators, m => m.Id == _existingUserId);
    }

    [Fact]
    public async Task AddModeratorAsync_ModeratorAdded()
    {
        // Act
        await _courseRepository.AddModeratorAsync(_existingCourseId, _existingUserId);
        var course = await _courseRepository.GetCourseWithModeratorsAsync(_existingCourseId);

        // Assert
        Assert.NotNull(course);
        Assert.Contains(course.Moderators, m => m.Id == _existingUserId);
    }

    [Fact]
    public async Task GetMostPopularCourses_ReturnsCourses()
    {
        // Act
        var popularCourses = await _courseRepository.GetMostPopularCourses(5);

        // Assert
        Assert.NotEmpty(popularCourses);
    }

    [Fact]
    public async Task GetRecommendedCourses_ReturnsGlowingCourses()
    {
        // Act
        var recommendedCourses = await _courseRepository.GetRecommendedCourses(5);

        // Assert
        Assert.NotEmpty(recommendedCourses);
        Assert.All(recommendedCourses, c => Assert.True(c.IsGlowing));
    }

    [Fact]
    public async Task GetCoursesAsync_NoFilters_ReturnsAllCourses()
    {
        // Arrange
        var request = new CatalogGetQueriesRequest
        {
            ElementsOnPage = 10
        };

        // Act
        var courses = await _courseRepository.GetCoursesAsync(request);

        // Assert
        Assert.Equal(2, courses.count);
    }

    [Fact]
    public async Task GetCoursesAsync_WithCategoryFilter_ReturnsFilteredCourses()
    {
        // Arrange
        var categoryId = _course.CategoryId;
        var request = new CatalogGetQueriesRequest
        {
            Category = categoryId,
            ElementsOnPage = 10
        };

        // Act
        var (courses, count) = await _courseRepository.GetCoursesAsync(request);

        // Assert
        Assert.Equal(1, count);
        Assert.Single(courses);
    }

    [Fact]
    public async Task GetCoursesAsync_WithPriceRangeFilter_ReturnsFilteredCourses()
    {
        // Arrange
        var request = new CatalogGetQueriesRequest
        {
            PriceFrom = 100,
            PriceTo = 500,
            ElementsOnPage = 10
        };

        // Act
        var courses = await _courseRepository.GetCoursesAsync(request);

        // Assert
        Assert.Equal(1, courses.count);
    }

    [Fact]
    public async Task GetCoursesAsync_WithActiveOnlyFilter_ReturnsOnlyActiveCourses()
    {
        // Arrange
        var request = new CatalogGetQueriesRequest
        {
            ActiveOnly = true,
            ElementsOnPage = 10
        };

        // Act
        var courses = await _courseRepository.GetCoursesAsync(request);

        // Assert
        Assert.Equal(1, courses.count);
    }

    [Fact]
    public async Task GetCoursesAsync_WithSearchQuery_ReturnsFilteredCourses()
    {
        // Arrange
        var searchQuery = _course.Title.Substring(0, 3);
        var request = new CatalogGetQueriesRequest
        {
            Q = searchQuery,
            ElementsOnPage = 10
        };

        // Act
        var (courses, count) = await _courseRepository.GetCoursesAsync(request);

        // Assert
        Assert.Equal(1, count);
        Assert.Single(courses);
    }

    [Fact]
    public async Task GetCoursesAsync_WithSortingByNew_ReturnsCoursesSortedByCreatedDate()
    {
        // Arrange
        var request = new CatalogGetQueriesRequest
        {
            Sort = "new",
            ElementsOnPage = 10
        };

        // Act
        var courses = await _courseRepository.GetCoursesAsync(request);

        // Assert
        Assert.Equal(2, courses.count);
    }

    [Fact]
    public async Task GetCoursesAsync_WithPagination_ReturnsCorrectPageOfCourses()
    {
        // Arrange
        var request = new CatalogGetQueriesRequest
        {
            ElementsOnPage = 1
        };

        // Act
        var courses = await _courseRepository.GetCoursesAsync(request);

        // Assert
        Assert.Single(courses.list);
    }

    private void ProduceTestData()
    {
        _course = _fixture.Build<Course>()
            .With(c => c.Title, "010e883d-496d-4b6b-80f0-dcf9b96e645b")
            .With(c => c.Id, 1)
            .With(c => c.IsArchived, false)
            .With(c => c.AuthorId, _existingUserId)
            .With(c => c.PriceRubles, 250)
            .With(c => c.CategoryId, 1)
            .Create();

        _course.Users.Add(new User { Id = _existingUserId, FirstName = "Troll", LastName = "Face" });

        var course = _fixture.Build<Course>()
            .With(c => c.Title, "85476ab4-11a1-473f-be5c-f2f60ef7ed89")
            .With(c => c.IsGlowing, true)
            .With(c => c.Id, 2)
            .With(c => c.IsArchived, true)
            .With(c => c.PriceRubles, 5000)
            .With(c => c.CategoryId, 2)
            .Create();

        _existingCourseId = _course.Id;
        _notExistingCourseId = 999;

        _context.Courses.Add(_course);
        _context.Courses.AddRange(course);
        _context.SaveChanges();
    }
}

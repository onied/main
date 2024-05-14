using AutoFixture;
using AutoMapper;
using Courses.Data.Models;
using Courses.Dtos.Catalog.Request;
using Courses.Dtos.Catalog.Response;
using Courses.Helpers;
using Courses.Profiles;
using Courses.Services.Abstractions;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using CatalogService = Courses.Services.CatalogService;
using Task = System.Threading.Tasks.Task;

namespace Tests.Courses.UnitTests.ServiceTests;

public class CatalogServiceTests
{
    private readonly CatalogService _service;
    private readonly IMapper _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new AppMappingProfile())));
    private readonly Fixture _fixture = new();
    private readonly Mock<ICourseRepository> _courseRepository = new();
    private readonly Mock<IUserRepository> _userRepository = new();

    public CatalogServiceTests()
    {
        _service = new CatalogService(
            _courseRepository.Object,
            _userRepository.Object,
            _mapper);
    }

    [Fact]
    public async Task Get_UsesCorrectOffsetAndLimit()
    {
        // Arrange
        var pageQuery = new CatalogGetQueriesRequest(); // Adjust as needed

        var courses = _fixture.Build<Course>()
            .CreateMany(3)
            .ToList();

        foreach (var course in courses)
        {
            var author = _fixture.Build<User>()
                .With(author1 => author1.Id, Guid.NewGuid)
                .Do(author1 => author1.Courses.Add(course))
                .Create();
            course.Author = author;
            course.AuthorId = author.Id;

            var category = _fixture.Build<Category>()
                .Do(category1 => category1.Courses.Add(course))
                .Create();
            course.Category = category;
            course.CategoryId = category.Id;
        }

        var userId = Guid.NewGuid();

        _courseRepository
            .Setup(e => e.GetCoursesAsync(pageQuery))
            .Returns(Task.FromResult((courses, courses.Count)));

        _userRepository
            .Setup(e => e.GetUserWithCoursesAsync(userId))
            .Returns(Task.FromResult(new User() ?? null));

        // Act
        var httpResult = await _service.Get(pageQuery, userId);

        // Assert
        var result = Assert.IsType<Ok<Page<CourseCardResponse>>>(httpResult);
        var page = result.Value;
        Assert.NotNull(page.Elements);
        Assert.NotEmpty(page.Elements);
        Assert.Equal(20, page.ElementsPerPage);
    }
}

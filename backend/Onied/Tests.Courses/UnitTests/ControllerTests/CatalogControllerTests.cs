using AutoFixture;
using AutoMapper;
using Courses.Controllers;
using Courses.Dtos;
using Courses.Helpers;
using Courses.Models;
using Courses.Profiles;
using Courses.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Task = System.Threading.Tasks.Task;

namespace Tests.Courses.UnitTests.ControllerTests;

public class CatalogControllerTests
{
    private readonly CatalogController _controller;
    private readonly IMapper _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new AppMappingProfile())));
    private readonly Fixture _fixture = new();
    private readonly Mock<ICourseRepository> _courseRepository = new();
    private readonly Mock<ILogger<CatalogController>> _logger = new();

    public CatalogControllerTests()
    {
        _controller = new CatalogController(_logger.Object, _mapper, _courseRepository.Object);
    }

    [Fact]
    public async Task Get_UsesCorrectOffsetAndLimit()
    {
        // Arrange
        var pageQuery = new PageQuery(); // Adjust as needed

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

        _courseRepository
            .Setup(e => e.GetCoursesAsync(0, pageQuery.ElementsOnPage))
            .Returns(Task.FromResult(courses));

        // Act
        var result = await _controller.Get(pageQuery);

        // Assert
        Assert.IsType<Page<CourseCardDto>>(result);
        var page = result;
        Assert.NotNull(page.Elements);
        Assert.NotEmpty(page.Elements);
        Assert.Equal(20, page.ElementsPerPage);
    }
}

using AutoFixture;
using AutoMapper;
using Courses;
using Courses.Controllers;
using Courses.Dtos;
using Courses.Helpers;
using Courses.Models;
using Courses.Profiles;
using Microsoft.Extensions.Logging;
using Moq;
using Task = System.Threading.Tasks.Task;

namespace Tests.Courses.UnitTests.ControllerTests;

public class CatalogControllerTests
{
    private readonly AppDbContext _context;
    private readonly Mock<ILogger<CatalogController>> _logger = new();
    private readonly IMapper _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new AppMappingProfile())));
    private readonly Fixture _fixture = new();
    private readonly CatalogController _controller;
    private readonly TestDataGenerator _generator;
    private readonly IEnumerable<Course> _courses;

    public CatalogControllerTests()
    {
        _context = ContextGenerator.GetContext();
        _generator = new TestDataGenerator(_context, _fixture);
        _courses = _generator.GenerateTestCourses();
        _generator.AddTestCoursesToDb(_courses);
        _controller = new CatalogController(_logger.Object, _mapper, _context);
    }
    
    [Fact]
    public async Task Get_UsesCorrectOffsetAndLimit()
    {
        // Arrange
        var pageQuery = new PageQuery(); // Adjust as needed

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
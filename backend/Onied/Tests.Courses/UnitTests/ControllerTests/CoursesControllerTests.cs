using AutoFixture;
using AutoMapper;
using Courses;
using Courses.Controllers;
using Courses.Dtos;
using Courses.Models;
using Courses.Profiles;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Task = System.Threading.Tasks.Task;

namespace Tests.Courses.UnitTests.ControllerTests;

public class CoursesControllerTests
{
    private readonly AppDbContext _context;
    private readonly Mock<ILogger<CoursesController>> _logger = new();
    private readonly IMapper _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new AppMappingProfile())));
    private readonly Fixture _fixture = new();
    private readonly CoursesController _controller;
    private readonly TestDataGenerator _generator;

    public CoursesControllerTests()
    {
        _context = ContextGenerator.GetContext();
        _generator = new TestDataGenerator(_context, _fixture);
        _generator.AddTestCoursesToDb(_generator.GenerateTestCourses());
        _controller = new CoursesController(_logger.Object, _mapper, _context);
    }
    
    [Theory]
    [InlineData(-1)]
    public async Task GetCourseHierarchy_ReturnsNotFound_WhenCourseNotExist(int courseId)
    {
        // Act
        var result = await _controller.GetCourseHierarchy(courseId);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }
    
    [Theory]
    [InlineData(1)]
    public async Task GetCourseHierarchy_ReturnsCourse(int courseId)
    {
        // Arrange
        var courses = _generator.GenerateTestCourses();

        // Act
        var result = await _controller.GetCourseHierarchy(courseId);

        // Assert
        var actionResult = Assert.IsType<ActionResult<CourseDto>>(result);
        var value = Assert.IsAssignableFrom<CourseDto>(
            actionResult.Value);
        Assert.Equal(value.Id, courseId);
        Assert.Equal(value.Modules.Count, courses.First().Modules.Count);
    }
    
    [Theory]
    [InlineData(1, -1)]
    public async Task GetSummaryBlock_ReturnsNotFound_WhenBlockNotExist(int courseId, int blockId)
    {
        // Act
        var result = await _controller.GetSummaryBlock(courseId, blockId);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }
    
    [Theory]
    [InlineData(-1, 1)]
    public async Task GetSummaryBlock_ReturnsNotFound_WhenCourseNotRight(int courseId, int blockId)
    {
        // Act
        var result = await _controller.GetVideoBlock(courseId, blockId);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }
    
    [Theory]
    [InlineData(1, 1)]
    public async Task GetSummaryBlock_ReturnsBlock(int courseId, int blockId)
    {
        // Act
        var result = await _controller.GetSummaryBlock(courseId, blockId);

        // Assert
        var actionResult = Assert.IsType<ActionResult<SummaryBlockDto>>(result);
        var value = Assert.IsAssignableFrom<SummaryBlockDto>(
            actionResult.Value);
        Assert.Equal(value.Id, blockId);
    }
    
    [Theory]
    [InlineData(1, -1)]
    public async Task GetVideoBlock_ReturnsNotFound_WhenBlockNotExist(int courseId, int blockId)
    {
        // Act
        var result = await _controller.GetVideoBlock(courseId, blockId);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }
    
    [Theory]
    [InlineData(-1, 2)]
    public async Task GetVideoBlock_ReturnsNotFound_WhenCourseNotRight(int courseId, int blockId)
    {
        // Act
        var result = await _controller.GetVideoBlock(courseId, blockId);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }
    
    [Theory]
    [InlineData(1, 2)]
    public async Task GetVideoBlock_ReturnsBlock(int courseId, int blockId)
    {
        // Act
        var result = await _controller.GetVideoBlock(courseId, blockId);

        // Assert
        var actionResult = Assert.IsType<ActionResult<VideoBlockDto>>(result);
        var value = Assert.IsAssignableFrom<VideoBlockDto>(
            actionResult.Value);
        Assert.Equal(value.Id, blockId);
    }
    
    [Theory]
    [InlineData(1, -1)]
    public async Task GetTasksBlock_ReturnsNotFound_WhenBlockNotExist(int courseId, int blockId)
    {
        // Act
        var result = await _controller.GetTaskBlock(courseId, blockId);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }
    
    [Theory]
    [InlineData(-1, 3)]
    public async Task GetTasksBlock_ReturnsNotFound_WhenCourseNotRight(int courseId, int blockId)
    {
        // Act
        var result = await _controller.GetTaskBlock(courseId, blockId);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }
    
    [Theory]
    [InlineData(1, 3)]
    public async Task GetTasksBlock_ReturnsBlock(int courseId, int blockId)
    {
        // Act
        var result = await _controller.GetTaskBlock(courseId, blockId);

        // Assert
        var actionResult = Assert.IsType<ActionResult<TasksBlockDto>>(result);
        var value = Assert.IsAssignableFrom<TasksBlockDto>(
            actionResult.Value);
        Assert.Equal(value.Id, blockId);
    }
    
    [Fact]
    public void Get_ReturnsRightString()
    {
        // Act
        var result = _controller.Get();

        // Assert
        Assert.Equal(result, "Under construction...");
    }
}
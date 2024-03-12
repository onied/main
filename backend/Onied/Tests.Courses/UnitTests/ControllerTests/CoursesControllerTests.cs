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
    private readonly IEnumerable<Course> _courses;

    public CoursesControllerTests()
    {
        _context = ContextGenerator.GetContext();
        _generator = new TestDataGenerator(_context, _fixture);
        _courses = _generator.GenerateTestCourses();
        _generator.AddTestCoursesToDb(_courses);
        _controller = new CoursesController(_logger.Object, _mapper, _context);
    }
    
    [Fact]
    public async Task GetPreviewCourse_ReturnsNotFound_WhenCourseNotExist()
    {
        // Arrange
        var courseId = -1;
        
        // Act
        var result = await _controller.GetCoursePreview(courseId);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }
    
    [Fact]
    public async Task GetCoursePreview_ReturnsCoursePreview_NotProgramVisible()
    {
        // Arrange
        var courseId = _courses.First().Id;
        
        // Act
        var result = await _controller.GetCoursePreview(courseId);

        // Assert
        var actionResult = Assert.IsType<ActionResult<PreviewDto>>(result);
        var value = Assert.IsAssignableFrom<PreviewDto>(
            actionResult.Value);
        Assert.Equal(value.Id, courseId);
        Assert.Equivalent(value.CourseAuthor, _mapper.Map<AuthorDto>(_courses.First().Author), true);
    }
    
    [Fact]
    public async Task GetCoursePreview_ReturnsCoursePreview_ProgramVisible()
    {
        // Arrange
        var course = _courses.First(course => course.IsProgramVisible);
        var courseId = course.Id;
        
        
        // Act
        var result = await _controller.GetCoursePreview(courseId);

        // Assert
        var actionResult = Assert.IsType<ActionResult<PreviewDto>>(result);
        var value = Assert.IsAssignableFrom<PreviewDto>(
            actionResult.Value);
        Assert.Equal(value.Id, courseId);
        Assert.Equivalent(value.CourseProgram, course.Modules.Select(module => module.Title));
    }
    
    [Fact]
    public async Task GetCourseHierarchy_ReturnsNotFound_WhenCourseNotExist()
    {
        // Arrange
        var courseId = -1;
        
        // Act
        var result = await _controller.GetCourseHierarchy(courseId);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }
    
    [Fact]
    public async Task GetCourseHierarchy_ReturnsCourse()
    {
        // Arrange
        var courseId = _courses.First().Id;
        
        // Act
        var result = await _controller.GetCourseHierarchy(courseId);

        // Assert
        var actionResult = Assert.IsType<ActionResult<CourseDto>>(result);
        var value = Assert.IsAssignableFrom<CourseDto>(
            actionResult.Value);
        Assert.Equal(value.Id, courseId);
        Assert.Equal(value.Modules.Count, _courses.First().Modules.Count);
    }
    
    [Fact]
    public async Task GetSummaryBlock_ReturnsNotFound_WhenBlockNotExist()
    {
        // Arrange
        var courseId = _courses.First().Id;
        var blockId = -1;
        
        // Act
        var result = await _controller.GetSummaryBlock(courseId, blockId);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }
    
    [Fact]
    public async Task GetSummaryBlock_ReturnsNotFound_WhenCourseNotRight()
    {
        // Arrange
        var courseId = -1;
        var blockId = _courses.First()
            .Modules.First()
            .Blocks.OfType<SummaryBlock>().First().Id;
        
        // Act
        var result = await _controller.GetVideoBlock(courseId, blockId);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }
    
    [Fact]
    public async Task GetSummaryBlock_ReturnsBlock()
    {
        // Arrange
        var courseId = _courses.First().Id;
        var blockId = _courses.First()
            .Modules.First()
            .Blocks.OfType<SummaryBlock>().First().Id;
        
        // Act
        var result = await _controller.GetSummaryBlock(courseId, blockId);

        // Assert
        var actionResult = Assert.IsType<ActionResult<SummaryBlockDto>>(result);
        var value = Assert.IsAssignableFrom<SummaryBlockDto>(
            actionResult.Value);
        Assert.Equal(value.Id, blockId);
    }
    
    [Fact]
    public async Task GetVideoBlock_ReturnsNotFound_WhenBlockNotExist()
    {
        // Arrange
        var courseId = _courses.First().Id;
        var blockId = -1;
        
        // Act
        var result = await _controller.GetVideoBlock(courseId, blockId);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }
    
    [Fact]
    public async Task GetVideoBlock_ReturnsNotFound_WhenCourseNotRight()
    {
        // Arrange
        var courseId = -1;
        var blockId = _courses.First()
            .Modules.First()
            .Blocks.OfType<VideoBlock>().First().Id;
        // Act
        var result = await _controller.GetVideoBlock(courseId, blockId);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }
    
    [Fact]
    public async Task GetVideoBlock_ReturnsBlock()
    {
        // Arrange
        var courseId = _courses.First().Id;
        var blockId = _courses.First()
            .Modules.First()
            .Blocks.OfType<VideoBlock>().First().Id;
        
        // Act
        var result = await _controller.GetVideoBlock(courseId, blockId);

        // Assert
        var actionResult = Assert.IsType<ActionResult<VideoBlockDto>>(result);
        var value = Assert.IsAssignableFrom<VideoBlockDto>(
            actionResult.Value);
        Assert.Equal(value.Id, blockId);
    }
    
    [Fact]
    public async Task GetTasksBlock_ReturnsNotFound_WhenBlockNotExist()
    {
        // Arrange
        var courseId = _courses.First().Id;
        var blockId = -1;
        
        // Act
        var result = await _controller.GetTaskBlock(courseId, blockId);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }
    
    [Fact]
    public async Task GetTasksBlock_ReturnsNotFound_WhenCourseNotRight()
    {
        // Arrange
        var courseId = -1;
        var blockId = _courses.First()
            .Modules.First()
            .Blocks.OfType<TasksBlock>().First().Id;
        
        // Act
        var result = await _controller.GetTaskBlock(courseId, blockId);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }
    
    [Fact]
    public async Task GetTasksBlock_ReturnsBlock()
    {
        // Arrange
        var courseId = _courses.First().Id;
        var blockId = _courses.First()
            .Modules.First()
            .Blocks.OfType<TasksBlock>().First().Id;
        
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
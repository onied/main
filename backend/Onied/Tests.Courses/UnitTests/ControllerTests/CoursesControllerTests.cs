using AutoFixture;
using AutoMapper;
using Courses;
using Courses.Controllers;
using Courses.Dtos;
using Courses.Models;
using Courses.Profiles;
using Courses.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Task = System.Threading.Tasks.Task;
using TaskProj = Courses.Models.Task;

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
    private readonly Mock<ICheckTasksService> _checkTasksService = new();

    public CoursesControllerTests()
    {
        _context = ContextGenerator.GetContext();
        _generator = new TestDataGenerator(_context, _fixture);
        _courses = _generator.GenerateTestCourses();
        _generator.AddTestCoursesToDb(_courses);
        _controller = new CoursesController(_logger.Object, _mapper, _context, _checkTasksService.Object);
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
        var course = _courses.First();
        var courseId = course.Id;
        
        // Act
        var result = await _controller.GetCoursePreview(courseId);

        // Assert
        var actionResult = Assert.IsType<ActionResult<PreviewDto>>(result);
        var value = Assert.IsAssignableFrom<PreviewDto>(
            actionResult.Value);
        Assert.Equal(courseId, value.Id);
        Assert.Equivalent(_mapper.Map<AuthorDto>(course.Author), value.CourseAuthor, true);
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
        Assert.Equal(courseId, value.Id);
        Assert.Equivalent(course.Modules.Select(module => module.Title), value.CourseProgram);
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
        Assert.Equal(courseId, value.Id);
        Assert.Equal(_courses.First().Modules.Count, value.Modules.Count);
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
        Assert.Equal(blockId, value.Id);
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
        Assert.Equal(blockId, value.Id);
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
        Assert.Equal(blockId, value.Id);
    }

    [Fact]
    public async Task GetTaskPointsStored_ReturnsNotFound_WhenBlockNotExist()
    {
        // Arrange
        var courseId = _courses.First().Id;
        var blockId = -1;
        
        // Act
        var result = await _controller.GetTaskPointsStored(courseId, blockId);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }
    
    [Fact]
    public async Task GetTaskPointsStored_ReturnsNotFound_WhenCourseNotRight()
    {
        // Arrange
        var courseId = -1;
        var blockId = _courses.First()
            .Modules.First()
            .Blocks.OfType<TasksBlock>().First().Id;
        
        // Act
        var result = await _controller.GetTaskPointsStored(courseId, blockId);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }
    
    [Fact]
    public async Task GetTaskPointsStored_ReturnsPointsRight()
    {
        // Arrange
        var courseId = _courses.First().Id;
        var block = _courses.First()
            .Modules.First()
            .Blocks.OfType<TasksBlock>().First();
        var blockId = block.Id;
        
        // Act
        var result = await _controller.GetTaskPointsStored(courseId, blockId);

        // Assert
        var actionResult = Assert.IsType<ActionResult<List<UserTaskPointsDto>>>(result);
        var value = Assert.IsAssignableFrom<List<UserTaskPointsDto>>(
            actionResult.Value);
        Assert.Equivalent(block.Tasks.Select(task => task.Points).OrderBy(x => x), 
            value.Select(x => x.Points).OrderBy(x => x));
    }
    
    [Fact]
    public async Task CheckTaskBlock_ReturnsNotFound_WhenBlockNotExist()
    {
        // Arrange
        var courseId = _courses.First().Id;
        var blockId = -1;
        
        // Act
        var result = await _controller.CheckTaskBlock(courseId, blockId, new List<UserInputDto>());

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }
    
    [Fact]
    public async Task CheckTaskBlock_ReturnsNotFound_WhenCourseNotRight()
    {
        // Arrange
        var courseId = -1;
        var blockId = _courses.First()
            .Modules.First()
            .Blocks.OfType<TasksBlock>().First().Id;
        
        // Act
        var result = await _controller.CheckTaskBlock(courseId, blockId, new List<UserInputDto>());

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }
    
    [Fact]
    public async Task CheckTaskBlock_ReturnsBadRequest_EmptyAnyUserInputDto()
    {
        // Arrange
        var courseId = _courses.First().Id;
        var block = _courses.First()
            .Modules.First()
            .Blocks.OfType<TasksBlock>().First();
        var blockId = block.Id;
        var inputsDto = _fixture.Build<UserInputDto>()
            .FromFactory(() => null)
            .CreateMany(1)
            .ToList();

        // Act
        var result = await _controller.CheckTaskBlock(courseId, blockId, inputsDto);

        // Assert
        Assert.IsType<BadRequestResult>(result.Result);
    }

    [Fact]
    public async Task CheckTaskBlock_ReturnsNotFound_NotTaskInBlock()
    {
        // Arrange
        var courseId = _courses.First().Id;
        var block = _courses.First()
            .Modules.First()
            .Blocks.OfType<TasksBlock>().First();
        var blockId = block.Id;
        var inputsDto = _fixture.Build<UserInputDto>()
            .With(input => input.TaskId, -1)
            .CreateMany(1)
            .ToList();

        // Act
        var result = await _controller.CheckTaskBlock(courseId, blockId, inputsDto);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result.Result);
    }
    
    [Fact]
    public async Task CheckTaskBlock_ReturnsBadRequest_NotTaskType()
    {
        // Arrange
        var courseId = _courses.First().Id;
        var block = _courses.First()
            .Modules.First()
            .Blocks.OfType<TasksBlock>().First();
        var blockId = block.Id;
        var task = block.Tasks.First();
        var inputsDto = _fixture.Build<UserInputDto>()
            .With(input => input.TaskId, task.Id)
            .With(input => input.TaskType, task.TaskType + 1 % 4)
            .CreateMany(1)
            .ToList();

        // Act
        var result = await _controller.CheckTaskBlock(courseId, blockId, inputsDto);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }
    
    [Fact]
    public async Task CheckTaskBlock_ReturnsListUserTaskPointsDto()
    {
        // Arrange
        var courseId = _courses.First().Id;
        var block = _courses.First()
            .Modules.First()
            .Blocks.OfType<TasksBlock>().First();
        var blockId = block.Id;
        var task = block.Tasks.First();
        var inputsDto = _fixture.Build<UserInputDto>()
            .With(input => input.TaskId, task.Id)
            .With(input => input.TaskType, task.TaskType)
            .CreateMany(1)
            .ToList();
        _checkTasksService.Setup(cts => cts.CheckTask(It.IsAny<TaskProj>(), It.IsAny<UserInputDto>()))
            .Returns(new UserTaskPoints
            {
                TaskId = task.Id,
            });

        // Act
        var result = await _controller.CheckTaskBlock(courseId, blockId, inputsDto);

        // Assert
        var actionResult = Assert.IsType<ActionResult<List<UserTaskPointsDto>>>(result);
        var value = Assert.IsAssignableFrom<List<UserTaskPointsDto>>(
            actionResult.Value);
        Assert.Equivalent(task.Id, 
            value.First().TaskId);
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
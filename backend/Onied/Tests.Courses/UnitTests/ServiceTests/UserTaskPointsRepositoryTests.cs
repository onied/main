using AutoFixture;
using Courses.Data;
using Courses.Data.Models;
using Courses.Services;
using Courses.Services.Abstractions;
using Tests.Courses.Helpers;
using Task = System.Threading.Tasks.Task;
using TaskProj = Courses.Data.Models.Task;

namespace Tests.Courses.UnitTests.ServiceTests;

public class UserTaskPointsRepositoryTests
{
    private readonly Fixture _fixture = new();
    private readonly AppDbContext _context;
    private readonly IUserTaskPointsRepository _userTaskPointsRepository;

    private Guid _existingUserId;
    private int _existingTaskId;
    private int _existingCourseId;
    private UserTaskPoints _existingUserTaskPoints = null!;

    public UserTaskPointsRepositoryTests()
    {
        _context = AppDbContextTest.GetContext();
        _userTaskPointsRepository = new UserTaskPointsRepository(_context);
        ProduceTestData();
    }

    [Fact]
    public async Task GetConcreteUserTaskPoints_ExistingUserAndTask_ReturnsUserTaskPoints()
    {
        // Act
        var result = await _userTaskPointsRepository.GetConcreteUserTaskPoints(_existingUserId, _existingTaskId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(_existingUserId, result.UserId);
        Assert.Equal(_existingTaskId, result.TaskId);
    }

    [Fact]
    public async Task GetConcreteUserTaskPoints_NonExistingUserAndTask_ReturnsNull()
    {
        // Act
        var result = await _userTaskPointsRepository.GetConcreteUserTaskPoints(Guid.NewGuid(), 999);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetUserTaskPointsByUserAndCourse_ReturnsUserTaskPoints()
    {
        // Act
        var result = await _userTaskPointsRepository.GetUserTaskPointsByUserAndCourse(_existingUserId, _existingCourseId);

        // Assert
        Assert.NotEmpty(result);
        Assert.All(result, tp => Assert.Equal(_existingUserId, tp.UserId));
        Assert.All(result, tp => Assert.Equal(_existingCourseId, tp.CourseId));
    }

    [Fact]
    public async Task StoreUserTaskPointsForConcreteUser_AddsNewAndUpdatesExistingPoints()
    {
        _context.ChangeTracker.Clear();

        // Arrange
        var existingCourse = await _context.Courses.FindAsync(_existingCourseId);
        var existingUser = await _context.Users.FindAsync(_existingUserId);
        var newTaskPoint = _fixture.Build<UserTaskPoints>()
            .With(tp => tp.User, existingUser)
            .With(tp => tp.UserId, existingUser!.Id)
            .With(tp => tp.Course, existingCourse)
            .With(tp => tp.CourseId, existingCourse!.Id)
            .With(tp => tp.TaskId, 2)
            .Create();

        var updateTaskPoint = _fixture.Build<UserTaskPoints>()
            .With(tp => tp.User, existingUser)
            .With(tp => tp.UserId, existingUser.Id)
            .With(tp => tp.Course, existingCourse)
            .With(tp => tp.CourseId, existingCourse.Id)
            .With(tp => tp.TaskId, _existingTaskId)
            .Create();

        var userTaskPointsList = new List<UserTaskPoints> { newTaskPoint, updateTaskPoint };

        // Act
        await _userTaskPointsRepository.StoreUserTaskPointsForConcreteUserAndBlock(userTaskPointsList, _existingUserId, _existingCourseId, 0); // BlockId not used

        // Assert
        var allPoints = await _userTaskPointsRepository.GetUserTaskPointsByUserAndCourse(_existingUserId, _existingCourseId);
        Assert.Contains(allPoints, tp => tp.TaskId == _existingTaskId);
    }

    [Fact]
    public async Task StoreConcreteUserTaskPoints_AddsNewTaskPoints()
    {
        // Arrange
        var task = new TaskProj { Id = 3, Title = "Title" };
        await _context.Tasks.AddAsync(task);
        await _context.SaveChangesAsync();

        var newUserTaskPoints = _fixture.Create<UserTaskPoints>();
        newUserTaskPoints.TaskId = task.Id;
        newUserTaskPoints.Task = task;

        // Act
        await _userTaskPointsRepository.StoreConcreteUserTaskPoints(newUserTaskPoints);

        // Assert
        var result = await _userTaskPointsRepository.GetConcreteUserTaskPoints(newUserTaskPoints.UserId, 3);
        Assert.NotNull(result);
        Assert.Equal(newUserTaskPoints.UserId, result.UserId);
        Assert.Equal(newUserTaskPoints.TaskId, result.TaskId);
    }

    [Fact]
    public async Task StoreConcreteUserTaskPoints_UpdatesExistingTaskPoints()
    {
        // Arrange
        _existingUserTaskPoints.Points = 100;

        // Act
        await _userTaskPointsRepository.StoreConcreteUserTaskPoints(_existingUserTaskPoints);

        // Assert
        var result = await _userTaskPointsRepository.GetConcreteUserTaskPoints(_existingUserId, _existingTaskId);
        Assert.NotNull(result);
        Assert.Equal(100, result.Points);
    }

    private void ProduceTestData()
    {
        _existingUserTaskPoints = _fixture.Build<UserTaskPoints>()
            .With(tp => tp.Points, 50)
            .Create();

        _context.UserTaskPoints.Add(_existingUserTaskPoints);
        _context.SaveChanges();

        _existingCourseId = _existingUserTaskPoints.CourseId;
        _existingTaskId = _existingUserTaskPoints.TaskId;
        _existingUserId = _existingUserTaskPoints.UserId;
    }
}

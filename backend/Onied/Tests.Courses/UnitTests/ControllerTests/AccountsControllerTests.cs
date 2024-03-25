using AutoFixture;
using Courses.Controllers;
using Courses.Models;
using Courses.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Task = System.Threading.Tasks.Task;

namespace Tests.Courses.UnitTests.ControllerTests;

public class AccountsControllerTests
{
    private readonly Fixture _fixture = new();
    private readonly Mock<IUserRepository> _userRepository = new();
    private readonly AccountsController _controller;

    public AccountsControllerTests()
    {
        _controller = new AccountsController(_userRepository.Object);
    }

    [Fact]
    public async Task GetCourses_ReturnsNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        var userId = Guid.NewGuid();

        // Act
        var result = await _controller.GetCourses(userId);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task GetCourses_ReturnsEmptyCourses_WhenUserExistsAndHasNoCourses()
    {
        // Arrange
        var user = _fixture.Build<User>()
            .With(u => u.Id, Guid.NewGuid)
            .Create();

        _userRepository.Setup(a => a.GetUserWithCoursesAsync(user.Id))
            .Returns(Task.FromResult(user)!);

        // Act
        var result = await _controller.GetCourses(user.Id);

        // Assert
        var actionResult = Assert.IsType<ActionResult<List<Course>>>(result);
        var actualCourses = Assert.IsAssignableFrom<List<Course>>(
            actionResult.Value);
        Assert.NotNull(actualCourses);
        Assert.Empty(actualCourses);
    }

    [Fact]
    public async Task GetCourses_ReturnsCourses_WhenUserExistsAndHasCourses()
    {
        // Arrange
        var user = _fixture.Build<User>()
            .With(u => u.Id, Guid.NewGuid())
            .Create();

        const int courseSequenceLength = 2;
        var courses = Enumerable.Range(1, courseSequenceLength)
            .Select(i => _fixture.Build<Course>()
                .With(c => c.Id, i)
                .Create())
            .ToList();
        foreach (var c in courses)
            user.Courses.Add(c);


        _userRepository.Setup(a => a.GetUserWithCoursesAsync(user.Id))
            .Returns(Task.FromResult(user)!);

        // Act
        var result = await _controller.GetCourses(user.Id);

        // Assert
        var actionResult = Assert.IsType<ActionResult<List<Course>>>(result);
        var actualCourses = Assert.IsAssignableFrom<List<Course>>(
            actionResult.Value);
        Assert.NotNull(actualCourses);
        Assert.Equal(courses, actualCourses);
    }
}

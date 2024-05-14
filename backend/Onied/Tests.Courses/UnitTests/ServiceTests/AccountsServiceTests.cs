using AutoFixture;
using AutoMapper;
using Courses.Data.Models;
using Courses.Dtos.Catalog.Response;
using Courses.Profiles;
using Courses.Services;
using Courses.Services.Abstractions;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using Task = System.Threading.Tasks.Task;

namespace Tests.Courses.UnitTests.ServiceTests;

public class AccountsServiceTests
{
    private readonly Fixture _fixture = new();
    private readonly IMapper _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new AppMappingProfile())));
    private readonly Mock<IUserRepository> _userRepository = new();
    private readonly AccountsService _service;

    public AccountsServiceTests()
    {
        _service = new AccountsService(_userRepository.Object, _mapper);
    }

    [Fact]
    public async Task GetCourses_ReturnsNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        var userId = Guid.NewGuid();

        // Act
        var result = await _service.GetCourses(userId);

        // Assert
        Assert.IsType<NotFound>(result);
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
        var result = await _service.GetCourses(user.Id);

        // Assert
        var actionResult = Assert.IsType<Ok<List<CourseCardResponse>>>(result);
        var actualCourses = Assert.IsAssignableFrom<List<CourseCardResponse>>(
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

        var expectedCourses = _mapper.Map<List<CourseCardResponse>>(courses);
        expectedCourses.ForEach(course => { course.IsOwned = true; });

        _userRepository.Setup(a => a.GetUserWithCoursesAsync(user.Id))
            .Returns(Task.FromResult(user)!);

        // Act
        var result = await _service.GetCourses(user.Id);

        // Assert
        var actionResult = Assert.IsType<Ok<List<CourseCardResponse>>>(result);
        var actualCourses = Assert.IsAssignableFrom<List<CourseCardResponse>>(
            actionResult.Value);
        Assert.NotNull(actualCourses);
        Assert.Equivalent(expectedCourses, actualCourses);
    }
}

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

public class TeachingServiceTests
{
    private readonly IMapper _mapper =
        new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new AppMappingProfile())));
    private readonly Mock<IUserRepository> _userRepository = new();
    private readonly TeachingService _service;

    public TeachingServiceTests()
    {
        _service = new TeachingService(
            _userRepository.Object,
            _mapper);
    }

    [Fact]
    public async Task GetAuthoredCourses_UserExists_ReturnsOkResultWithCourses()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User();
        user.TeachingCourses.Add(new Course());

        _userRepository.Setup(repo => repo.GetUserWithTeachingCoursesAsync(userId))
            .ReturnsAsync(user);

        // Act
        var result = await _service.GetAuthoredCourses(userId);

        // Assert
        var okResult = Assert.IsType<Ok<List<CourseCardResponse>>>(result);
        var response = Assert.IsType<List<CourseCardResponse>>(okResult.Value);
        Assert.Single(response);
    }

    [Fact]
    public async Task GetAuthoredCourses_UserDoesNotExist_ReturnsNotFound()
    {
        // Act
        var result = await _service.GetAuthoredCourses(Guid.NewGuid());

        // Assert
        Assert.IsType<NotFound>(result);
    }

    [Fact]
    public async Task GetModeratedCourses_UserExists_ReturnsOkResultWithCourses()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User();
        user.ModeratingCourses.Add(new Course());

        _userRepository.Setup(repo => repo.GetUserWithModeratingCoursesAsync(userId))
            .ReturnsAsync(user);

        // Act
        var result = await _service.GetModeratedCourses(userId);

        // Assert
        var okResult = Assert.IsType<Ok<List<CourseCardResponse>>>(result);
        var response = Assert.IsType<List<CourseCardResponse>>(okResult.Value);
        Assert.Single(response);
    }

    [Fact]
    public async Task GetModeratedCourses_UserDoesNotExist_ReturnsNotFound()
    {
        // Act
        var result = await _service.GetModeratedCourses(Guid.NewGuid());

        // Assert
        Assert.IsType<NotFound>(result);
    }
}

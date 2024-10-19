using AutoFixture;
using AutoMapper;
using Courses.Data.Models;
using Courses.Dtos.Catalog.Response;
using Courses.Services;
using Courses.Services.Abstractions;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using Task = System.Threading.Tasks.Task;

namespace Tests.Courses.UnitTests.ServiceTests;

public class LandingPageContentServiceTests
{
    private readonly Fixture _fixture = new();
    private readonly Mock<ICourseRepository> _courseRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly ILandingPageContentService _landingPageContentService;

    public LandingPageContentServiceTests()
    {
        _courseRepositoryMock = new Mock<ICourseRepository>();
        _mapperMock = new Mock<IMapper>();
        _landingPageContentService = new LandingPageContentService(_courseRepositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task GetMostPopularCourses_WithCourses_ReturnsOkResult()
    {
        // Arrange
        var courses = _fixture.CreateMany<Course>(3).ToList();
        var courseCardResponses = _fixture.CreateMany<CourseCardResponse>(3).ToList();

        _courseRepositoryMock
            .Setup(repo => repo.GetMostPopularCourses(It.IsAny<int>()))
            .ReturnsAsync(courses);

        _mapperMock
            .Setup(m => m.Map<List<CourseCardResponse>>(It.IsAny<List<Course>>()))
            .Returns(courseCardResponses);

        // Act
        var result = await _landingPageContentService.GetMostPopularCourses(3);

        // Assert
        Assert.IsType<Results<Ok<List<CourseCardResponse>>, NotFound>>(result);
    }

    [Fact]
    public async Task GetMostPopularCourses_WithoutCourses_ReturnsNotFound()
    {
        // Arrange
        _courseRepositoryMock
            .Setup(repo => repo.GetMostPopularCourses(It.IsAny<int>()))
            .ReturnsAsync(new List<Course>());

        // Act
        var result = await _landingPageContentService.GetMostPopularCourses(3);

        // Assert
        Assert.IsType<Results<Ok<List<CourseCardResponse>>, NotFound>>(result);
        // Not Found
    }

    [Fact]
    public async Task GetRecommendedCourses_WithCourses_ReturnsOkResult()
    {
        // Arrange
        var courses = _fixture.CreateMany<Course>(2).ToList();
        var courseCardResponses = _fixture.CreateMany<CourseCardResponse>(2).ToList();

        _courseRepositoryMock
            .Setup(repo => repo.GetRecommendedCourses(It.IsAny<int>()))
            .ReturnsAsync(courses);

        _mapperMock
            .Setup(m => m.Map<List<CourseCardResponse>>(It.IsAny<List<Course>>()))
            .Returns(courseCardResponses);

        // Act
        var result = await _landingPageContentService.GetRecommendedCourses(2);

        // Assert
        Assert.IsType<Results<Ok<List<CourseCardResponse>>, NotFound>>(result);
    }

    [Fact]
    public async Task GetRecommendedCourses_WithoutCourses_ReturnsNotFound()
    {
        // Arrange
        _courseRepositoryMock
            .Setup(repo => repo.GetRecommendedCourses(It.IsAny<int>()))
            .ReturnsAsync(new List<Course>());

        // Act
        var result = await _landingPageContentService.GetRecommendedCourses(2);

        // Assert
        Assert.IsType<Results<Ok<List<CourseCardResponse>>, NotFound>>(result);
        // Not Found
    }
}

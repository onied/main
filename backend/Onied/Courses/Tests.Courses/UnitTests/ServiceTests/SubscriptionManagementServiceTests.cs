using System.Net;
using Courses.Data.Models;
using Courses.Dtos;
using Courses.Services;
using Courses.Services.Abstractions;
using Courses.Services.Producers.CourseUpdatedProducer;
using Moq;
using Tests.Courses.Helpers;
using Task = System.Threading.Tasks.Task;

namespace Tests.Courses.UnitTests.ServiceTests;

public class SubscriptionManagementServiceTests
{
    private readonly Mock<IHttpClientFactory> _httpClientFactory = new();
    private readonly Mock<IUserRepository> _userRepository = new();
    private readonly Mock<ICourseRepository> _courseRepository = new();
    private readonly Mock<ICourseUpdatedProducer> _courseUpdatedProducer = new();
    private readonly SubscriptionManagementService _service;

    public SubscriptionManagementServiceTests()
    {
        _service = new SubscriptionManagementService(
            _httpClientFactory.Object,
            _userRepository.Object,
            _courseRepository.Object,
            _courseUpdatedProducer.Object);
    }

    [Fact]
    public async Task VerifyGivingCertificatesAsync_SubscriptionExists_ReturnsTrue()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var subscription = new SubscriptionRequestDto { CertificatesEnabled = true };

        _userRepository.Setup(repo => repo.GetUserWithTeachingCoursesAsync(userId))
            .ReturnsAsync(new User());

        SetupHttpClient(subscription);

        // Act
        var result = await _service.VerifyGivingCertificatesAsync(userId);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task VerifyGivingCertificatesAsync_SubscriptionDoesNotExist_ReturnsFalse()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var subscription = new SubscriptionRequestDto { CertificatesEnabled = false };

        _userRepository.Setup(repo => repo.GetUserWithTeachingCoursesAsync(userId))
            .ReturnsAsync(new User());

        SetupHttpClient(subscription);

        // Act
        var result = await _service.VerifyGivingCertificatesAsync(userId);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task VerifyCreatingCoursesAsync_SubscriptionExists_ReturnsTrue()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var subscription = new SubscriptionRequestDto { CourseCreatingEnabled = true };

        _userRepository.Setup(repo => repo.GetUserWithTeachingCoursesAsync(userId))
            .ReturnsAsync(new User());

        SetupHttpClient(subscription);

        // Act
        var result = await _service.VerifyCreatingCoursesAsync(userId);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task VerifyCreatingCoursesAsync_SubscriptionDoesNotExist_ReturnsFalse()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var subscription = new SubscriptionRequestDto { CourseCreatingEnabled = false };

        _userRepository.Setup(repo => repo.GetUserWithTeachingCoursesAsync(userId))
            .ReturnsAsync(new User());

        SetupHttpClient(subscription);

        // Act
        var result = await _service.VerifyCreatingCoursesAsync(userId);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task SetAuthorCoursesCertificatesEnabled_UserExists_SetsCertificates()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User();
        user.TeachingCourses.Add(new Course { HasCertificates = true });

        _userRepository.Setup(repo => repo.GetUserWithTeachingCoursesAsync(userId))
            .ReturnsAsync(user);

        // Act
        await _service.SetAuthorCoursesCertificatesEnabled(userId, true);

        // Assert
        Assert.True(user.TeachingCourses.First().HasCertificates);
        _courseRepository.Verify(repo => repo.UpdateCourseAsync(It.IsAny<Course>()), Times.Once);
        _courseUpdatedProducer.Verify(p => p.PublishAsync(It.IsAny<Course>()), Times.Once);
    }

    [Fact]
    public async Task SetAuthorCoursesCertificatesEnabled_UserDoesNotExist_ThrowsArgumentException()
    {
        // Arrange
        var userId = Guid.NewGuid();

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.SetAuthorCoursesCertificatesEnabled(userId, true));
    }

    [Fact]
    public async Task SetAuthorCoursesHighlightingEnabled_UserExists_SetsHighlighting()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User();
        user.TeachingCourses.Add(new Course { IsGlowing = true });

        _userRepository.Setup(repo => repo.GetUserWithTeachingCoursesAsync(userId))
            .ReturnsAsync(user);

        // Act
        await _service.SetAuthorCoursesHighlightingEnabled(userId, true);

        // Assert
        Assert.True(user.TeachingCourses.First().IsGlowing);
        _courseRepository.Verify(repo => repo.UpdateCourseAsync(It.IsAny<Course>()), Times.Once);
    }

    [Fact]
    public async Task SetAuthorCoursesHighlightingEnabled_UserDoesNotExist_ThrowsArgumentException()
    {
        // Arrange
        var userId = Guid.NewGuid();

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.SetAuthorCoursesHighlightingEnabled(userId, true));
    }

    [Fact]
    public async Task GetSubscriptionAsync_UserExists_ReturnsSubscription()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var subscription = new SubscriptionRequestDto();

        SetupHttpClient(subscription);

        // Act
        var result = await _service.GetSubscriptionAsync(userId);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetSubscriptionAsync_UserDoesNotExist_ReturnsNull()
    {
        // Arrange
        var userId = Guid.NewGuid();

        var client = new HttpClient(new MockHttpMessageHandler(HttpStatusCode.BadRequest));
        client.BaseAddress = new UriBuilder().Uri;

        _httpClientFactory.Setup(f => f.CreateClient(It.IsAny<string>()))
            .Returns(client);

        // Act
        var result = await _service.GetSubscriptionAsync(userId);

        // Assert
        Assert.Null(result);
    }

    private void SetupHttpClient(SubscriptionRequestDto subscription)
    {
        var client = new HttpClient(new MockHttpMessageHandler(HttpStatusCode.OK, subscription));
        client.BaseAddress = new UriBuilder().Uri;
        _httpClientFactory.Setup(f => f.CreateClient(It.IsAny<string>()))
            .Returns(client);
    }
}

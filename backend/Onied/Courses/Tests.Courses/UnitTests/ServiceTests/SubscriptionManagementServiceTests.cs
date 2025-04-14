using AutoMapper;
using Courses.Data.Models;
using Courses.Dtos;
using Courses.Profiles;
using Courses.Services;
using Courses.Services.Abstractions;
using Courses.Services.Producers.CourseUpdatedProducer;
using Google.Protobuf;
using Grpc.Core;
using Moq;
using PurchasesGrpc;
using Tests.Courses.Helpers;
using Task = System.Threading.Tasks.Task;

namespace Tests.Courses.UnitTests.ServiceTests;

public class SubscriptionManagementServiceTests
{
    private readonly Mock<ICourseRepository> _courseRepository = new();
    private readonly Mock<ICourseUpdatedProducer> _courseUpdatedProducer = new();

    private readonly SubscriptionRequestDto _genericSubscription = new()
    {
        AdsEnabled = false, CertificatesEnabled = false, CourseCreatingEnabled = false,
        CoursesHighlightingEnabled = false, Price = 0, Id = 1, Title = "Generic Subscription"
    };

    private readonly Mock<SubscriptionService.SubscriptionServiceClient> _grpcClient = new();

    private readonly IMapper _mapper =
        new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new AppMappingProfile())));

    private readonly SubscriptionManagementService _service;
    private readonly Mock<IUserRepository> _userRepository = new();

    public SubscriptionManagementServiceTests()
    {
        _service = new SubscriptionManagementService(
            _userRepository.Object,
            _courseRepository.Object,
            _courseUpdatedProducer.Object, _grpcClient.Object, _mapper);
    }

    [Fact]
    public async Task VerifyGivingCertificatesAsync_SubscriptionExists_ReturnsTrue()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _genericSubscription.CertificatesEnabled = true;

        _userRepository.Setup(repo => repo.GetUserWithTeachingCoursesAsync(userId))
            .ReturnsAsync(new User());

        SetupGrpcClient(_genericSubscription);

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

        _userRepository.Setup(repo => repo.GetUserWithTeachingCoursesAsync(userId))
            .ReturnsAsync(new User());

        SetupGrpcClient(_genericSubscription);

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
        _genericSubscription.CourseCreatingEnabled = true;

        _userRepository.Setup(repo => repo.GetUserWithTeachingCoursesAsync(userId))
            .ReturnsAsync(new User());

        SetupGrpcClient(_genericSubscription);

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

        _userRepository.Setup(repo => repo.GetUserWithTeachingCoursesAsync(userId))
            .ReturnsAsync(new User());

        SetupGrpcClient(_genericSubscription);

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

        SetupGrpcClient(_genericSubscription);

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

        _grpcClient.Setup(client =>
            client.GetActiveSubscriptionAsync(
                new GetActiveSubscriptionRequest { UserId = ByteString.CopyFrom(userId.ToByteArray()) }, null, null,
                CancellationToken.None)).Throws(new RpcException(new Status(StatusCode.NotFound, "User not found.")));

        // Act
        var result = await _service.GetSubscriptionAsync(userId);

        // Assert
        Assert.Null(result);
    }

    private void SetupGrpcClient(SubscriptionRequestDto subscription)
    {
        var mockCall = CallHelpers.CreateAsyncUnaryCall(new GetActiveSubscriptionReply
        {
            Id = subscription.Id,
            Title = subscription.Title,
            CoursesHighlightingEnabled = subscription.CoursesHighlightingEnabled, AdsEnabled = subscription.AdsEnabled,
            CertificatesEnabled = subscription.CertificatesEnabled,
            CourseCreatingEnabled = subscription.CourseCreatingEnabled, Price = subscription.Price
        });

        _grpcClient.Setup(client =>
                client.GetActiveSubscriptionAsync(It.IsAny<GetActiveSubscriptionRequest>(), null, null,
                    CancellationToken.None))
            .Returns(mockCall);
    }
}

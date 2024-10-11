using System.ComponentModel.DataAnnotations;
using AutoFixture;
using Courses.Services.Abstractions;
using Courses.Services.Producers.NotificationSentProducer;
using MassTransit;
using MassTransit.Data.Messages;
using Microsoft.Extensions.Logging;
using Moq;
using Task = System.Threading.Tasks.Task;

namespace Tests.Courses.UnitTests.ServiceBusTests;

public class NotificationSentProducerTests
{
    private readonly Fixture _fixture = new();
    
    private readonly Mock<ILogger<NotificationSentProducer>> _logger = new();
    private readonly Mock<IUserRepository> _userRepository = new();
    private readonly Mock<INotificationPreparerService> _notificationPreparerService = new();
    private readonly Mock<IPublishEndpoint> _publishEndpoint = new();

    private NotificationSentProducer GetProducer() => new(
        _logger.Object,
        _userRepository.Object,
        _notificationPreparerService.Object,
        _publishEndpoint.Object);
    
        
    [Fact]
    public async Task PublishForOne_ValidMessage_Success()
    {
        // Arrange
        var notification = _fixture
            .Build<NotificationSent>()
            .With(property => property.Title, GetRandomString(6))
            .Create();

        Queue<NotificationSent> queue = new();

        _notificationPreparerService
            .Setup(preparerService => preparerService.PrepareNotification(notification))
            .Returns(notification);

        _publishEndpoint
            .Setup(pe => pe.Publish(It.IsAny<NotificationSent>(), It.IsAny<CancellationToken>()))
            .Callback((NotificationSent notificationSent, CancellationToken _) => queue.Enqueue(notificationSent));
        
        var producer = GetProducer();
        
        // Act
        await producer.PublishForOne(notification);
        
        // Assert
        _publishEndpoint.Verify(
            mr => mr.Publish(
                It.IsAny<NotificationSent>(), 
                It.IsAny<CancellationToken>()), 
            Times.Once());
        var actualNotification = Assert.Single(queue);
        Assert.Equivalent(notification, actualNotification);
        
    }
    
    [Fact]
    public async Task PublishForOne_UserIdIsQuidEmpty_Failure()
    {
        // Arrange
        var notification = _fixture
            .Build<NotificationSent>()
            .With(u => u.UserId, Guid.Empty)
            .Create();

        _notificationPreparerService
            .Setup(preparerService => preparerService.PrepareNotification(notification))
            .Returns(notification);

        
        var producer = GetProducer();
        
        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(
            async () => await producer.PublishForOne(notification));
    }

    private string GetRandomString(int length)
    {
        var bytes = new byte[length];
        Random.Shared.NextBytes(bytes);
        return Convert.ToBase64String(bytes);
    }
}

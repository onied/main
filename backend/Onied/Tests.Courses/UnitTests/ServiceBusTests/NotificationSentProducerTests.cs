using System.ComponentModel.DataAnnotations;
using AutoFixture;
using Courses.Data.Models;
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
            .With(n => n.Title, Common.RandomUtils.Utils.GetRandomString(6))
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
    public async Task PublishForOne_UserIdIsGuidEmpty_Failure()
    {
        // Arrange
        var notification = _fixture
            .Build<NotificationSent>()
            .With(n => n.UserId, Guid.Empty)
            .Create();

        _notificationPreparerService
            .Setup(preparerService => preparerService.PrepareNotification(notification))
            .Returns(notification);

        
        var producer = GetProducer();
        
        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(
            async () => await producer.PublishForOne(notification));
    }
    
    [Fact]
    public async Task PublishForAll_NoUsers_NoMessages()
    {
        // Arrange
        var notification = _fixture
            .Build<NotificationSent>()
            .With(n => n.UserId, Guid.Empty)
            .With(n => n.Title, Common.RandomUtils.Utils.GetRandomString(6))
            .Create();

        Queue<NotificationSent> queue = new();

        _userRepository
            .Setup(repo => repo.GetUsersWithConditionAsync(null))
            .ReturnsAsync([]);

        _notificationPreparerService
            .Setup(preparerService => preparerService.PrepareNotification(It.IsAny<NotificationSent>()))
            .Returns((NotificationSent n) => n);

        _publishEndpoint
            .Setup(pe => pe.Publish(It.IsAny<NotificationSent>(), It.IsAny<CancellationToken>()))
            .Callback((NotificationSent notificationSent, CancellationToken _) => queue.Enqueue(notificationSent));
        
        var producer = GetProducer();
        
        // Act
        await producer.PublishForAll(notification);
        
        // Assert
        _publishEndpoint.Verify(
            mr => mr.Publish(
                It.IsAny<NotificationSent>(), 
                It.IsAny<CancellationToken>()), 
            Times.Never());

        Assert.Empty(queue);
    }
    
    [Fact]
    public async Task PublishForAll_ValidMessage_Success()
    {
        // Arrange
        const int expectedCount = 30;
        
        var notification = _fixture
            .Build<NotificationSent>()
            .With(n => n.Title, Common.RandomUtils.Utils.GetRandomString(6))
            .Create();
        
        var users = _fixture
            .Build<User>()
            .With(u => u.Id, Guid.NewGuid())
            .CreateMany(expectedCount)
            .ToList();

        Queue<NotificationSent> queue = new();

        _userRepository
            .Setup(repo => repo.GetUsersWithConditionAsync(null))
            .ReturnsAsync(users);

        _notificationPreparerService
            .Setup(preparerService => preparerService.PrepareNotification(It.IsAny<NotificationSent>()))
            .Returns((NotificationSent n) => n);

        _publishEndpoint
            .Setup(pe => pe.Publish(It.IsAny<NotificationSent>(), It.IsAny<CancellationToken>()))
            .Callback((NotificationSent notificationSent, CancellationToken _) => queue.Enqueue(notificationSent));
        
        var producer = GetProducer();
        
        // Act
        await producer.PublishForAll(notification);
        
        // Assert
        _publishEndpoint.Verify(
            mr => mr.Publish(
                It.IsAny<NotificationSent>(), 
                It.IsAny<CancellationToken>()), 
            Times.Exactly(expectedCount));
        
        Assert.Equal(expectedCount, queue.Count);
        Assert.All(queue,
            actual => Assert.True(
                actual.Title == notification.Title 
                && actual.Message == notification.Message
                && actual.Image == notification.Image));
        Assert.Equal(queue.Select(n => n.UserId), users.Select(u => u.Id));
        
    }
}

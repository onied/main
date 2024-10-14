using AutoFixture;
using Common.RandomUtils;
using Courses.Services;
using MassTransit.Data.Messages;

namespace Tests.Courses.UnitTests.ServiceTests;

public class NotificationPreparerServiceTests
{
    private readonly Fixture _fixture = new();

    private readonly NotificationPreparerService _service = new();

    [Fact]
    public void PrepareNotification_ValidMessage_NoChanges()
    {
        // Arrange
        var notification = _fixture
            .Build<NotificationSent>()
            .With(notification => notification.Title, Utils.GetRandomString(6))
            .Create();

        // Act
        var actualNotification = _service.PrepareNotification(notification);

        // Assert
        Assert.Equivalent(notification, actualNotification);
    }

    [Fact]
    public void PrepareNotification_TitleOvercome_TextChanged()
    {
        // Arrange
        var notification = _fixture
            .Build<NotificationSent>()
            .With(notification => notification.Title, Utils.GetRandomString(101))
            .Create();

        // Act
        var actualNotification = _service.PrepareNotification(notification);

        // Assert
        Assert.EndsWith("...", actualNotification.Title);
        Assert.Equal(100, actualNotification.Title.Length);
    }

    [Fact]
    public void PrepareNotification_MessageOvercome_TextChanged()
    {
        // Arrange
        var notification = _fixture
            .Build<NotificationSent>()
            .With(notification => notification.Message, Utils.GetRandomString(351))
            .Create();

        // Act
        var actualNotification = _service.PrepareNotification(notification);

        // Assert
        Assert.EndsWith("...", actualNotification.Message);
        Assert.Equal(350, actualNotification.Message.Length);
    }

    [Fact]
    public void PrepareNotification_TitleAndMessageOvercome_TextChanged()
    {
        // Arrange
        var notification = _fixture
            .Build<NotificationSent>()
            .With(notification => notification.Title, Utils.GetRandomString(101))
            .With(notification => notification.Message, Utils.GetRandomString(351))
            .Create();

        // Act
        var actualNotification = _service.PrepareNotification(notification);

        // Assert
        Assert.EndsWith("...", actualNotification.Title);
        Assert.Equal(100, actualNotification.Title.Length);
        Assert.EndsWith("...", actualNotification.Message);
        Assert.Equal(350, actualNotification.Message.Length);
    }
}

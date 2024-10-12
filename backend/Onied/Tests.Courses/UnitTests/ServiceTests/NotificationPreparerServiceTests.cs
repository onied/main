using AutoFixture;
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
        var notification =
            _fixture
                .Build<NotificationSent>()
                .With(notification => notification.Title, GetRandomString(6))
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
        var notification =
            _fixture
                .Build<NotificationSent>()
                .With(notification => notification.Title, GetRandomString(101))
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
        var notification =
            _fixture
                .Build<NotificationSent>()
                .With(notification => notification.Message, GetRandomString(351))
                .Create();

        // Act
        var actualNotification = _service.PrepareNotification(notification);

        // Assert
        Assert.EndsWith("...", actualNotification.Message);
        Assert.Equal(350, actualNotification.Message.Length);
    }

    private string GetRandomString(int length)
    {
        var bytes = new byte[length];
        Random.Shared.NextBytes(bytes);
        return Convert.ToBase64String(bytes);
    }
}

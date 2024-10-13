using Courses.Services;
using MassTransit.Data.Messages;

namespace Tests.Courses.UnitTests.ServiceTests;

public class NotificationPreparerServiceTests
{
    private readonly NotificationPreparerService _notificationPreparerService;

    public NotificationPreparerServiceTests()
    {
        _notificationPreparerService = new NotificationPreparerService();
    }

    [Fact]
    public void PrepareNotification_TitleWithinLimit_ReturnsSameTitleAndMessage()
    {
        // Arrange
        var notification = new NotificationSent(
            Title: "Short Title",
            Message: "This is a short message.",
            UserId: Guid.NewGuid()
        );

        // Act
        var result = _notificationPreparerService.PrepareNotification(notification);

        // Assert
        Assert.Equal("Short Title", result.Title);
        Assert.Equal("This is a short message.", result.Message);
    }

    [Fact]
    public void PrepareNotification_TitleExceedsLimit_TrimsTitle()
    {
        // Arrange
        var notification = new NotificationSent(
            Title: new string('A', 105),
            Message: "This is a valid message.",
            UserId: Guid.NewGuid()
        );

        // Act
        var result = _notificationPreparerService.PrepareNotification(notification);

        // Assert
        Assert.Equal(new string('A', 97) + "...", result.Title); // Should trim to 97 chars and add "..."
        Assert.Equal("This is a valid message.", result.Message);
    }

    [Fact]
    public void PrepareNotification_MessageExceedsLimit_TrimsMessage()
    {
        // Arrange
        var notification = new NotificationSent(
            Title: "Valid Title",
            Message: new string('B', 360),
            UserId: Guid.NewGuid()
        );

        // Act
        var result = _notificationPreparerService.PrepareNotification(notification);

        // Assert
        Assert.Equal("Valid Title", result.Title);
        Assert.Equal(new string('B', 347) + "...", result.Message); // Should trim to 347 chars and add "..."
    }

    [Fact]
    public void PrepareNotification_TitleAndMessageBothExceedLimits_TrimsBoth()
    {
        // Arrange
        var notification = new NotificationSent(
            Title: new string('C', 105),
            Message: new string('D', 360),
            UserId: Guid.NewGuid()
        );

        // Act
        var result = _notificationPreparerService.PrepareNotification(notification);

        // Assert
        Assert.Equal(new string('C', 97) + "...", result.Title); // Should trim title
        Assert.Equal(new string('D', 347) + "...", result.Message); // Should trim message
    }
}

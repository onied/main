using System.ComponentModel.DataAnnotations;

namespace MassTransit.Data.Messages;

public record NotificationSent(
    [MaxLength(100)] string Title,
    [MaxLength(350)] string Message,
    Guid UserId,
    [Url][MaxLength(2048)] string Image = "https://www.emojiall.com/en/svg-to-png/twitter/1920/1f479.png"
);

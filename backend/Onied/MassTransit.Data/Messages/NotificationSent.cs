using System.ComponentModel.DataAnnotations;
using Common.Validation.Attributes;

namespace MassTransit.Data.Messages;

public record NotificationSent(
    [MaxLength(100)] string Title,
    [MaxLength(350)] string Message,
    [property: GuidNotEmpty(ErrorMessage = "UserId cannot be Guid.Empty")]
    Guid UserId,
    [Url][MaxLength(2048)] string Image = "https://www.emojiall.com/en/svg-to-png/twitter/1920/1f479.png"
)
{
    public static NotificationSent Normalize(NotificationSent notification)
        => notification with
        {
            Title = notification.Title.Length > 100
                ? string.Concat(notification.Title.AsSpan(0, 97), "...")
                : notification.Title,
            Message = notification.Message.Length > 350
                ? string.Concat(notification.Message.AsSpan(0, 347), "...")
                : notification.Message,
            Image = notification.Image.Length > 2048
                ? "https://www.emojiall.com/en/svg-to-png/twitter/1920/1f479.png"
                : notification.Image
        };
};

using System.ComponentModel.DataAnnotations;

namespace Notifications.Dtos.Responses;

public class NotificationResponseDto
{
    public int Id { get; set; }

    [Url]
    [MaxLength(2048)]
    public string Img { get; set; } = null!;

    [MaxLength(100)]
    public string Title { get; set; } = null!;

    [MaxLength(350)]
    public string Message { get; set; } = null!;

    public bool IsRead { get; set; }
}

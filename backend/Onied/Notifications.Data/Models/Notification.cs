using System.ComponentModel.DataAnnotations;

namespace Notifications.Data.Models;

public class Notification
{
    public int Id { get; set; }

    [Url]
    [MaxLength(2048)]
    public string Image { get; set; } = null!;

    [MaxLength(100)]
    public string Title { get; set; } = null!;

    [MaxLength(350)]
    public string Message { get; set; } = null!;

    public bool IsRead { get; set; }
    public Guid UserId { get; set; }
}

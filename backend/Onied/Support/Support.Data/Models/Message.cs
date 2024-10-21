using System.ComponentModel.DataAnnotations;

namespace Support.Data.Models;

public class Message
{
    public Guid Id { get; set; }

    public Chat Chat { get; set; } = null!;
    public Guid ChatId { get; set; }

    /// <summary>
    ///     UserId can be a user from Users service or from Support table.
    /// </summary>
    public Guid UserId { get; set; }

    public DateTime CreatedAt { get; set; }

    /// <summary>
    ///     Null if it wasn't read by anyone other than UserId. Date and time otherwise.
    /// </summary>
    public DateTime? ReadAt { get; set; }

    [MaxLength(1024)]
    public string MessageContent { get; set; } = null!;

    public bool IsSystem { get; set; }
}

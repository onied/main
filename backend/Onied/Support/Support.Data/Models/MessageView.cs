using System.ComponentModel.DataAnnotations;

namespace Support.Data.Models;

/// <summary>
/// This object should map to the VIEW in the database
/// </summary>
public class MessageView
{
    public Guid Id { get; set; }

    public Chat Chat { get; set; } = null!;
    public Guid ChatId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? ReadAt { get; set; }

    [MaxLength(1024)]
    public string MessageContent { get; set; } = null!;

    public bool IsSystem { get; set; }

    public int? SupportNumber { get; set; }
}

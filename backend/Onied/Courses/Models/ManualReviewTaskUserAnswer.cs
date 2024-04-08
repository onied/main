using System.ComponentModel.DataAnnotations;

namespace Courses.Models;

public class ManualReviewTaskUserAnswer
{
    public Guid Id { get; set; }

    [MinLength(1)]
    [MaxLength(15000)]
    public string Content { get; set; } = null!;

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public int TaskId { get; set; }
    public Task Task { get; set; } = null!;
}

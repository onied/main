using System.ComponentModel.DataAnnotations;

namespace Courses.Data.Models;

public class ManualReviewTaskUserAnswer : UserTaskPoints
{
    public Guid ManualReviewTaskUserAnswerId { get; set; }

    [MinLength(1)]
    [MaxLength(15000)]
    public string Content { get; set; } = null!;
}

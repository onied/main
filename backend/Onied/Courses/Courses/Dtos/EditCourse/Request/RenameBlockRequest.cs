using System.ComponentModel.DataAnnotations;

namespace Courses.Dtos.EditCourse.Request;

public class RenameBlockRequest
{
    public int BlockId { get; set; }

    [MinLength(1)]
    [MaxLength(200)]
    public string Title { get; set; } = null!;
}

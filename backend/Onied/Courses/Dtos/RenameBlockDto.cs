using System.ComponentModel.DataAnnotations;

namespace Courses.Dtos;

public class RenameBlockDto
{
    public int BlockId { get; set; }

    [MinLength(1)]
    [MaxLength(200)]
    public string Title { get; set; } = null!;
}

using System.ComponentModel.DataAnnotations;

namespace Courses.Models;

public class Block
{
    public int Id { get; set; }

    public int ModuleId { get; set; }
    public Module Module { get; set; } = null!;

    [MinLength(1)]
    [MaxLength(200)]
    public string Title { get; set; } = null!;

    public virtual BlockType BlockType { get; set; }
}

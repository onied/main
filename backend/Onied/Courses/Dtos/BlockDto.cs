using Courses.Models;

namespace Courses.Dtos;

public class BlockDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public BlockType BlockType { get; set; }
    public bool Completed { get; set; }
}

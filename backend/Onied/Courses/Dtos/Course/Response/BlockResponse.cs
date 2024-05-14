using Courses.Data.Models;

namespace Courses.Dtos.Course.Response;

public class BlockResponse
{
    public int Id { get; set; }
    public int Index { get; set; }
    public string Title { get; set; } = null!;
    public BlockType BlockType { get; set; }
    public bool Completed { get; set; }
}

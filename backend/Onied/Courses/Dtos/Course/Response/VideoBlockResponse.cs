using Courses.Models;

namespace Courses.Dtos.Course.Response;

public class VideoBlockResponse
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public BlockType BlockType { get; set; }
    public bool IsCompleted { get; set; }
    public string Href { get; set; } = null!;
}

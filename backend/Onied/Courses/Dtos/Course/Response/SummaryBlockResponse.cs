using Courses.Data.Models;

namespace Courses.Dtos.Course.Response;

public class SummaryBlockResponse
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public BlockType BlockType { get; set; }
    public bool IsCompleted { get; set; }
    public string? MarkdownText { get; set; }
    public string? FileName { get; set; }
    public string? FileHref { get; set; }
}

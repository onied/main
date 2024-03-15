using Courses.Models;

namespace Courses.Dtos;

public class SummaryBlockDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public BlockType BlockType { get; set; }
    public bool IsCompleted { get; set; }
    public string? MarkdownText { get; set; }
    public string? FileName { get; set; }
    public string? FileHref { get; set; }
}

namespace Courses.Models;

public class SummaryBlock : Block
{
    public string MarkdownText { get; set; } = null!;
    public string FileName { get; set; } = null!;
    public string FileHref { get; set; } = null!;
}
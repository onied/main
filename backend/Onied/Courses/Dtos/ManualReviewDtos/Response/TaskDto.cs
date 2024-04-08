namespace Courses.Dtos.ManualReviewDtos.Response;

public class TaskDto
{
    public BlockDto Block { get; set; } = null!;
    public int Index { get; set; }
    public string Title { get; set; } = null!;
    public int MaxPoints { get; set; }
}

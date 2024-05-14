namespace Courses.Dtos.ManualReview.Response;

public class TaskResponse
{
    public BlockResponse Block { get; set; } = null!;
    public int Index { get; set; }
    public string Title { get; set; } = null!;
    public int MaxPoints { get; set; }
}

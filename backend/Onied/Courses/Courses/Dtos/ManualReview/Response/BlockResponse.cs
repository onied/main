namespace Courses.Dtos.ManualReview.Response;

public class BlockResponse
{
    public ModuleResponse Module { get; set; } = null!;
    public int Index { get; set; }
    public string Title { get; set; } = null!;
}

namespace Courses.Dtos.ManualReview.Response;

public class ModuleResponse
{
    public CourseResponse Course { get; set; } = null!;
    public int Index { get; set; }
    public string Title { get; set; } = null!;
}

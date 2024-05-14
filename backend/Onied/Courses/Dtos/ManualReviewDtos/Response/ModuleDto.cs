namespace Courses.Dtos.ManualReviewDtos.Response;

public class ModuleDto
{
    public CourseDto Course { get; set; } = null!;
    public int Index { get; set; }
    public string Title { get; set; } = null!;
}

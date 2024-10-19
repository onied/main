namespace Courses.Dtos.Course.Response;

public class CourseResponse
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public List<ModuleResponse> Modules { get; init; } = new();
}

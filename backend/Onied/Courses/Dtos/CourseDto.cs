namespace Courses.Dtos;

public class CourseDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public List<ModuleDto> Modules { get; init; } = new();
}

namespace Courses.Dtos;

public class CourseDto
{
    public string Title { get; set; } = null!;
    public List<ModuleDto> Modules { get; } = new();
}
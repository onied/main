namespace Courses.Dtos.ModeratorDtos.Response;

public class CourseStudentsDto
{
    public int CourseId { get; set; }
    public string Title { get; set; } = default!;
    public List<StudentDto> Students { get; set; } = new();
}

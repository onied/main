namespace Courses.Dtos.Moderator.Response;

public class CourseStudentsResponse
{
    public int CourseId { get; set; }
    public string Title { get; set; } = default!;
    public List<StudentResponse> Students { get; set; } = new();
}

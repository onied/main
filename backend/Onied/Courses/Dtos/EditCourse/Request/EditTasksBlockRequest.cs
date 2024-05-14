using Courses.Models;

namespace Courses.Dtos.EditCourse.Request;

public class EditTasksBlockRequest
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public BlockType BlockType { get; set; }
    public List<EditTaskRequest> Tasks { get; set; } = new();
}

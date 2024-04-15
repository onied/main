using Courses.Models;

namespace Courses.Dtos;

public class EditTasksBlockDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public BlockType BlockType { get; set; }
    public List<EditTaskDto> Tasks { get; set; } = new();
}

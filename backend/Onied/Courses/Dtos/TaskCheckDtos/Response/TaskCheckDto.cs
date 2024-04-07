namespace Courses.Dtos.TaskCheckDtos;

public class TaskCheckDto
{
    public TaskDto Task { get; set; } = null!;
    public string Contents { get; set; } = null!;
    public bool Checked { get; set; }
    public int Points { get; set; }
}

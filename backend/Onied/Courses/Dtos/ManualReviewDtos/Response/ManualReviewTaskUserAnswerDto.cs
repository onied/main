namespace Courses.Dtos.ManualReviewDtos.Response;

public class ManualReviewTaskUserAnswerDto
{
    public TaskDto Task { get; set; } = null!;
    public string Content { get; set; } = null!;
    public bool Checked { get; set; }
    public int Points { get; set; }
}

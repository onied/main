namespace Courses.Dtos.ManualReview.Response;

public class ManualReviewTaskUserAnswerResponse
{
    public TaskResponse Task { get; set; } = null!;
    public string Content { get; set; } = null!;
    public bool Checked { get; set; }
    public int Points { get; set; }
}

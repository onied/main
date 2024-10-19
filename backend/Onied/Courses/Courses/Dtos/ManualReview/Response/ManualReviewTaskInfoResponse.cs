namespace Courses.Dtos.ManualReview.Response;

public class ManualReviewTaskInfoResponse
{
    public Guid Index { get; set; }
    public string Title { get; set; } = null!;
    public string BlockTitle { get; set; } = null!;
    public string ModuleTitle { get; set; } = null!;
}

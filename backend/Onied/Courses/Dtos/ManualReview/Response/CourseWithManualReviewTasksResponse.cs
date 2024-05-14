namespace Courses.Dtos.ManualReview.Response;

public class CourseWithManualReviewTasksResponse
{
    public int CourseId { get; set; }
    public string Title { get; set; } = null!;
    public List<ManualReviewTaskInfoResponse> TasksToCheck { get; set; } = null!;
}

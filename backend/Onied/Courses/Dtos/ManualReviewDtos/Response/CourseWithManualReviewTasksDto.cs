namespace Courses.Dtos.ManualReviewDtos.Response;

public class CourseWithManualReviewTasksDto
{
    public int CourseId { get; set; }
    public string Title { get; set; } = null!;
    public List<ManualReviewTaskInfoDto> TasksToCheck { get; set; } = null!;
}

namespace Courses.Dtos.ManualReviewDtos.Response;

public class ManualReviewTaskInfoDto
{
    public Guid Index { get; set; }
    public string Title { get; set; } = null!;
    public string BlockTitle { get; set; } = null!;
    public string ModuleTitle { get; set; } = null!;
}

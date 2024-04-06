namespace Courses.Dtos;

public class EditAnswersDto
{
    public int Id { get; set; }
    public string? Description { get; set; }
    public string? Answer { get; set; }
    public bool? IsCorrect { get; set; }
    public bool IsNew { get; set; }
}

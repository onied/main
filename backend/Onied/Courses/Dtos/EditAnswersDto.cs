namespace Courses.Dtos;

public class EditAnswersDto
{
    public int Id { get; set; }
    public string Description { get; set; } = null!;
    public string Answer { get; set; } = null!;
    public bool IsCaseSensitive { get; set; }
    public bool IsCorrect { get; set; }
    public bool IsNew { get; set; }

}

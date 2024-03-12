namespace Courses.Dtos;

public class CourseCardDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string PictureHref { get; set; } = null!;
    public int Price { get; set; }
    public CategoryDto Category { get; set; } = null!;
    public AuthorDto Author { get; set; } = null!;
    public bool IsArchived { get; set; }
    public bool IsGlowing { get; set; }
}
namespace Courses.Dtos;

public class PreviewDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string PictureHref { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int HoursCount { get; set; }
    public int Price { get; set; }
    public CategoryDto Category { get; set; } = null!;
    public AuthorDto CourseAuthor { get; set; } = null!;
    public bool IsArchived { get; set; }
    public bool HasCertificates { get; set; }
    public List<string>? CourseProgram { get; set; }
}

using Courses.Models;

namespace Courses.Dtos;

public class CoursePreviewDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string PictureHref { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int HoursCount { get; set; }
    public int Price { get; set; }
    public bool IsArchived { get; set; }
    public CategoryDto Category { get; set; }
    public AuthorDto Author { get; set; }
    public List<string> CourseProgram { get; } = new();
}
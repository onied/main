using Courses.Dtos.Catalog.Response;

namespace Courses.Dtos.Course.Response;

public class PreviewResponse
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string PictureHref { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int HoursCount { get; set; }
    public int Price { get; set; }
    public CategoryResponse Category { get; set; } = null!;
    public AuthorResponse CourseAuthor { get; set; } = null!;
    public bool IsArchived { get; set; }
    public bool HasCertificates { get; set; }
    public List<string>? CourseProgram { get; set; }

    public bool IsOwned { get; set; }
}

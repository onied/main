namespace Courses.Dtos.Catalog.Response;

public class CourseCardResponse
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string PictureHref { get; set; } = null!;
    public int Price { get; set; }
    public CategoryResponse Category { get; set; } = null!;
    public AuthorResponse Author { get; set; } = null!;
    public bool IsArchived { get; set; }
    public bool IsGlowing { get; set; }

    public bool IsOwned { get; set; }
}

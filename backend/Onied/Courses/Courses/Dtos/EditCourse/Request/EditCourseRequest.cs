using System.ComponentModel.DataAnnotations;

namespace Courses.Dtos.EditCourse.Request;

public class EditCourseRequest
{
    public int CategoryId { get; set; }

    [MinLength(1)]
    [MaxLength(200)]
    public string Title { get; set; } = null!;

    [Url]
    [MaxLength(2048)]
    public string PictureHref { get; set; } = null!;

    [MinLength(1)]
    [MaxLength(15000)]
    public string Description { get; set; } = null!;

    [Range(0, 35000)]
    public int HoursCount { get; set; }

    [Range(0, 1000000)]
    public int Price { get; set; }

    public bool IsArchived { get; set; }

    public bool HasCertificates { get; set; }

    public bool IsProgramVisible { get; set; }
}

using System.ComponentModel.DataAnnotations;

namespace Courses.Dtos.EditCourse.Request;

public class RenameModuleRequest
{
    public int ModuleId { get; set; }

    [MinLength(1)]
    [MaxLength(200)]
    public string Title { get; set; } = null!;
}

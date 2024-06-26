using System.ComponentModel.DataAnnotations;

namespace Courses.Data.Models;

public class VideoBlock : Block
{
    [Url]
    [MaxLength(2048)]
    public string Url { get; set; } = null!;
}

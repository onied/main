using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Support.Data.Models;

public class File
{
    public Guid Id { get; set; }

    [MaxLength(128)]
    public string Filename { get; set; } = null!;

    [Url]
    [MaxLength(256)]
    public string FileUrl { get; set; } = null!;

    public Guid MessageId { get; set; }
}

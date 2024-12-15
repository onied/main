using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Support.Data.Models;

public class File
{
    public Guid Id { get; set; }

    public string Filename { get; set; } = null!;

    [Url]
    public string FileUrl { get; set; } = null!;

    public Guid MessageId { get; set; }
}

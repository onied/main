using System.ComponentModel.DataAnnotations;

namespace Courses.Models;

public class SummaryBlock : Block, IValidatableObject
{
    [MaxLength(15000)]
    public string? MarkdownText { get; set; }

    [MaxLength(255)]
    public string? FileName { get; set; }

    [Url]
    [MaxLength(2048)]
    public string? FileHref { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (FileName == null != (FileHref == null))
            yield return new ValidationResult("File name requires file href, and vice-versa.",
                new[] { nameof(FileName), nameof(FileHref) });
        if (MarkdownText == null && FileName == null && FileHref == null)
            yield return new ValidationResult(
                "At least one of the following should be provided: Markdown text or file name and href.");
    }
}

using System.ComponentModel.DataAnnotations;

namespace Purchases.Dtos;

public class SubscriptionDto
{
    public int Id { get; set; }

    [MaxLength(200)]
    public string Title { get; set; } = null!;

    public bool CoursesHighlightingEnabled { get; set; }
    public bool AdsEnabled { get; set; }
    public bool CertificatesEnabled { get; set; }
    public bool CourseCreatingEnabled { get; set; }

    [Range(0, 1000000)]
    public decimal Price { get; set; }
}

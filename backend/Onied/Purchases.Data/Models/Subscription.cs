using System.ComponentModel.DataAnnotations;

namespace Purchases.Data.Models;

public class Subscription
{
    public int Id { get; set; }
    public string Title { get; set; }
    public bool CoursesHighlightingEnabled { get; set; }
    public bool AdsEnabled { get; set; }
    public bool CertificatesEnabled { get; set; }

    [Range(-1, int.MaxValue)]
    public int ActiveCoursesNumber { get; set; }

    [Range(0, 1000000)]
    public decimal Price { get; set; }

    public List<User> Users { get; set; } = null!;
}

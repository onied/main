namespace Purchases.Data.Models;

public class Subscription
{
    public int Id { get; set; }
    public bool CoursesHighlightingEnabled { get; set; }
    public bool AdsEnabled { get; set; }
    public bool CertificatesEnabled { get; set; }
    public int ActiveCoursesNumber { get; set; }
    public decimal Price { get; set; }
}

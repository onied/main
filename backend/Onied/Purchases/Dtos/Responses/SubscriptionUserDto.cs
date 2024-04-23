namespace Purchases.Dtos.Responses;

public class SubscriptionUserDto
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;
    public DateTime EndDate { get; set; }
    public bool AutoRenewalEnabled { get; set; }
    public bool CoursesHighlightingEnabled { get; set; }
    public bool AdsEnabled { get; set; }
    public bool CertificatesEnabled { get; set; }
    public bool AutoTestsReview { get; set; }

    public int ActiveCoursesNumber { get; set; }
    public int StudentsOnCourseLimit { get; set; }

    public decimal Price { get; set; }
}

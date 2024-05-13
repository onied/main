namespace MassTransit.Data.Messages;

public class SubscriptionChanged
{
    public Guid UserId { get; set; }
    public bool CoursesHighlightingEnabled { get; set; }
    public bool AdsEnabled { get; set; }
    public bool CertificatesEnabled { get; set; }
    public bool CourseCreatingEnabled { get; set; }
}

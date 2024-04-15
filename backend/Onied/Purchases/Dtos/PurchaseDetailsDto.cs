using Purchases.Data.Models;

namespace Purchases.Dtos;

public class PurchaseDetailsDto
{
    public PurchaseType PurchaseType { get; set; }
    public CourseDto? Course { get; set; }
    public SubscriptionDto? Subscription { get; set; }

    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool? AutoRenewalEnabled { get; set; }
}

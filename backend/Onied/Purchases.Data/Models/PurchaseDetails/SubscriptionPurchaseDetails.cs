namespace Purchases.Data.Models.PurchaseDetails;

public class SubscriptionPurchaseDetails : PurchaseDetails
{
    public int SubscriptionId { get; set; }
    public Subscription Subscription { get; set; } = null!;

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool AutoRenewalEnabled { get; set; }
}

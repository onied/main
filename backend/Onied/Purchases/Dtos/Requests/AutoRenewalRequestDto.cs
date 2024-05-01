namespace Purchases.Dtos.Requests;

public class AutoRenewalRequestDto
{
    public int SubscriptionId { get; set; }
    public bool AutoRenewal { get; set; }
}

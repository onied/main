namespace Purchases.Data.Models;

public class User
{
    public Guid Id { get; set; }

    public int SubscriptionId { get; set; }
    public Subscription Subscription { get; set; } = null!;
}

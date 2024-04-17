using MassTransit.Data.Enums;

namespace Purchases.Dtos;

public class PurchaseTokenInfo
{
    public int Id { get; set; }
    public PurchaseType PurchaseType { get; set; }
    public Guid UserId { get; set; }
    public int? CourseId { get; set; }
    public int? SubscriptionId { get; set; }
    public DateTime? Expires { get; set; }
}

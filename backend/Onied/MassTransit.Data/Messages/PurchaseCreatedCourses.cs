using MassTransit.Data.Enums;

namespace MassTransit.Data.Messages;

public class PurchaseCreatedCourses(PurchaseCreated purchaseCreated)
{
    public int Id { get; set; } = purchaseCreated.Id;
    public Guid UserId { get; set; } = purchaseCreated.UserId;
    public PurchaseType PurchaseType { get; set; } = purchaseCreated.PurchaseType;
    public int? CourseId { get; set; } = purchaseCreated.CourseId;
    public int? SubscriptionId { get; set; } = purchaseCreated.SubscriptionId;
    public string Token { get; set; } = purchaseCreated.Token;
}

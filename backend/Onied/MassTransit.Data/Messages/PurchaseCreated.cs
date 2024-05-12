using MassTransit.Data.Enums;

namespace MassTransit.Data.Messages;

public record PurchaseCreated(
    Guid UserId,
    PurchaseType PurchaseType,
    int? CourseId,
    int? SubscriptionId,
    string Token);

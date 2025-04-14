namespace MassTransit.Data.Messages;

public record SubscriptionChangeFailed(
    int OldSubscriptionId,
    int PurchaseId,
    Guid UserId,
    string ErrorMessage);

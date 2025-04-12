using MassTransit.Data.Enums;

namespace MassTransit.Data.Messages;

public record PurchaseCreatedCourses(
    int Id,
    Guid UserId,
    PurchaseType PurchaseType,
    int? CourseId,
    int? SubscriptionId,
    string Token);

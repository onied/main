using Purchases.Data.Models;

namespace Purchases.Dtos.Requests;

public record PurchaseRequestDto(
    Guid? UserId,
    decimal Price,
    CardInfoDto CardInfo,
    PurchaseType PurchaseType,
    int? CourseId,
    int? SubscriptionId);

using Purchases.Data.Enums;
using Purchases.Data.Models;

namespace Purchases.Dtos.Responses;

public record PreparedPurchaseResponseDto(
    string Title,
    decimal Price,
    PurchaseType PurchaseType = PurchaseType.Any);

using Purchases.Data.Models;

namespace Purchases.Dtos;

public record PreparedPurchaseResponseDto(
    string Title,
    decimal Price,
    PurchaseType PurchaseType = PurchaseType.Any);

using Purchases.Data.Models;

namespace Purchases.Dtos;

public class PurchaseRequestDto
{
    public PurchaseType PurchaseType { get; set; }
    public decimal Price { get; set; }
    public CardInfoDto CardInfo { get; set; } = null!;
}

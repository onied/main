using Purchases.Data.Models;

namespace Purchases.Dtos;

public class PurchaseRequestDto
{
    public string Title { get; set; } = null!;
    public decimal Price { get; set; }
    public PurchaseType PurchaseType { get; set; }
}

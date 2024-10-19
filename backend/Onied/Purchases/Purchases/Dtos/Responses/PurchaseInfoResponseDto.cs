namespace Purchases.Dtos.Responses;

public class PurchaseInfoResponseDto
{
    public int Id { get; set; }

    public PurchaseDetailsDto PurchaseDetails { get; set; } = null!;

    public decimal Price { get; set; }
}

namespace Purchases.Data.Models;

public class Purchase
{
    public int Id { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public PurchaseDetails.PurchaseDetails PurchaseDetails { get; set; } = null!;

    public decimal Price { get; set; }

    public string? Token { get; set; }
}

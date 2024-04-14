namespace Purchases.Data.Models;

public class Purchase
{
    public int Id { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public int PurchaseDetailsId { get; set; }
    public PurchaseDetails PurchaseDetails { get; set; } = null!;

    public PurchaseType PurchaseType { get; set; }
    public decimal Price { get; set; }
}

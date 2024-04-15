using Purchases.Data.Enums;

namespace Purchases.Data.Models.PurchaseDetails;

public class PurchaseDetails
{
    public int Id { get; set; }
    public PurchaseType PurchaseType { get; set; }
}

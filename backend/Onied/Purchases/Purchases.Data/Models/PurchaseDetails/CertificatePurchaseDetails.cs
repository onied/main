namespace Purchases.Data.Models.PurchaseDetails;

public class CertificatePurchaseDetails : PurchaseDetails
{
    public int CourseId { get; set; }
    public Course Course { get; set; } = null!;
}

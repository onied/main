using Purchases.Data.Models;

namespace Purchases.Dtos;

public class PurchaseDetailsDto
{
    public PurchaseType PurchaseType { get; set; }
    public CourseDto? Course { get; set; }
}

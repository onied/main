using Purchases.Abstractions;
using Purchases.Data.Models;
using Purchases.Data.Models.PurchaseDetails;

namespace Purchases.Services;

public class PurchaseTokenService(IJwtTokenService jwtTokenService) : IPurchaseTokenService
{
    public string GetToken(Purchase purchase)
    {
        var claims = new Dictionary<string, object?>()
        {
            { "Id", purchase.Id },
            { "PurchaseType", purchase.PurchaseDetails.PurchaseType },
            { "UserId", purchase.UserId },
            {
                "CourseId",
                purchase.PurchaseDetails is CoursePurchaseDetails pd1
                    ? pd1.CourseId
                    : (purchase.PurchaseDetails as CertificatePurchaseDetails)?.CourseId
            },
            { "SubscriptionId", (purchase.PurchaseDetails as SubscriptionPurchaseDetails)?.SubscriptionId },
            { "Expires", (purchase.PurchaseDetails as SubscriptionPurchaseDetails)?.EndDate }
        };

        return jwtTokenService.GenerateToken(claims);
    }
}

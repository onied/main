using System.Text.Json;
using Purchases.Data.Enums;
using Purchases.Data.Models;
using Purchases.Data.Models.PurchaseDetails;
using Purchases.Dtos;
using Purchases.Services.Abstractions;

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

    public PurchaseTokenInfo ConvertToPurchaseTokenInfo(string token)
    {
        var claims = new Dictionary<string, object?>()
        {
            { "Id", 0 },
            { "PurchaseType", PurchaseType.Any },
            { "UserId", Guid.Empty },
            { "CourseId", 0 },
            { "SubscriptionId", 0 },
            { "Expires", DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds() }
        };

        var json = jwtTokenService.DecodeToken(claims, token);
        return JsonSerializer.Deserialize<PurchaseTokenInfo>(json)!;
    }
}

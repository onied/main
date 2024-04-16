using Purchases.Data.Models;
using Purchases.Dtos;

namespace Purchases.Abstractions;

public interface IPurchaseTokenService
{
    public string GetToken(Purchase purchase);
    public PurchaseTokenInfo ConvertToPurchaseTokenInfo(string token);
}

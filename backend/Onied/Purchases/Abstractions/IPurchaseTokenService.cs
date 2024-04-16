using Purchases.Data.Models;

namespace Purchases.Abstractions;

public interface IPurchaseTokenService
{
    public string GetToken(Purchase purchase);
}

using Purchases.Data.Enums;
using Purchases.Dtos.Requests;

namespace Purchases.Services.Abstractions;

public interface IValidatePurchaseService
{
    public Task<IResult?> ValidatePurchase(PurchaseRequestDto dto, PurchaseType purchaseType);
}

using Purchases.Data.Enums;
using Purchases.Dtos.Requests;

namespace Purchases.Services.Abstractions;

public interface IPurchaseManagementService
{
    public Task<IResult?> ValidatePurchase(PurchaseRequestDto dto, PurchaseType purchaseType);
}

using Purchases.Dtos.Requests;

namespace Purchases.Services.Abstractions;

public interface IPurchaseService
{
    public Task<IResult> GetPurchasesByUser(Guid userId);
    public Task<IResult> Verify(VerifyTokenRequestDto dto);
}

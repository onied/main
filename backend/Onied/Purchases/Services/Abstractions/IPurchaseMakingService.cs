using Purchases.Dtos.Requests;

namespace Purchases.Services.Abstractions;

public interface IPurchaseMakingService
{
    public Task<IResult> GetCoursePreparedPurchase(int courseId);
    public Task<IResult> MakeCoursePurchase(PurchaseRequestDto dto, Guid userId);
    public Task<IResult> GetCertificatePreparedPurchase(int courseId);
    public Task<IResult> MakeCertificatePurchase(PurchaseRequestDto dto, Guid userId);
    public Task<IResult> GetSubscriptionPreparedPurchase(int subscriptionId);
    public Task<IResult> MakeSubscriptionPurchase(PurchaseRequestDto dto, Guid userId);
}

using Microsoft.AspNetCore.Mvc;
using Purchases.Dtos.Requests;
using Purchases.Services.Abstractions;

namespace Purchases.Controllers;

[Route("api/v1/purchases/new")]
public class PurchasesMakingController(
    IPurchaseMakingService purchaseMakingService
) : ControllerBase
{
    [HttpGet("course")]
    public async Task<IResult> GetCoursePreparedPurchase([FromQuery] int courseId)
        => await purchaseMakingService.GetCertificatePreparedPurchase(courseId);

    [HttpPost("course")]
    public async Task<IResult> MakeCoursePurchase([FromBody] PurchaseRequestDto dto, [FromQuery] Guid userId)
        => await purchaseMakingService.MakeCoursePurchase(dto, userId);

    [HttpGet("certificate")]
    public async Task<IResult> GetCertificatePreparedPurchase([FromQuery] int courseId)
        => await purchaseMakingService.GetCertificatePreparedPurchase(courseId);

    [HttpPost("certificate")]
    public async Task<IResult> MakeCertificatePurchase([FromBody] PurchaseRequestDto dto, [FromQuery] Guid userId)
        => await purchaseMakingService.MakeCoursePurchase(dto, userId);

    [HttpGet("subscription")]
    public async Task<IResult> GetSubscriptionPreparedPurchase([FromQuery] int subscriptionId)
        => await purchaseMakingService.GetCertificatePreparedPurchase(subscriptionId);

    [HttpPost("subscription")]
    public async Task<IResult> MakeSubscriptionPurchase([FromBody] PurchaseRequestDto dto, [FromQuery] Guid userId)
        => await purchaseMakingService.MakeCoursePurchase(dto, userId);
}

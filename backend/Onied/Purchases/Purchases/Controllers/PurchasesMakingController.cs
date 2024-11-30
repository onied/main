using MediatR;
using Microsoft.AspNetCore.Mvc;
using Purchases.Commands;
using Purchases.Dtos.Requests;
using Purchases.Queries;

namespace Purchases.Controllers;

[Route("api/v1/purchases/new")]
public class PurchasesMakingController(ISender sender) : ControllerBase
{
    [HttpGet("course")]
    public async Task<IResult> GetCoursePreparedPurchase([FromQuery] int courseId)
        => await sender.Send(new GetCoursePreparedPurchaseQuery(courseId));

    [HttpPost("course")]
    public async Task<IResult> MakeCoursePurchase([FromBody] PurchaseRequestDto dto, [FromQuery] Guid userId)
        => await sender.Send(new MakeCoursePurchaseCommand(dto, userId));

    [HttpGet("certificate")]
    public async Task<IResult> GetCertificatePreparedPurchase([FromQuery] int courseId)
        => await sender.Send(new GetCertificatePreparedPurchaseQuery(courseId));

    [HttpPost("certificate")]
    public async Task<IResult> MakeCertificatePurchase([FromBody] PurchaseRequestDto dto, [FromQuery] Guid userId)
        => await sender.Send(new MakeCertificatePurchaseCommand(dto, userId));

    [HttpGet("subscription")]
    public async Task<IResult> GetSubscriptionPreparedPurchase([FromQuery] int subscriptionId)
        => await sender.Send(new GetSubscriptionPreparedPurchaseQuery(subscriptionId));

    [HttpPost("subscription")]
    public async Task<IResult> MakeSubscriptionPurchase([FromBody] PurchaseRequestDto dto, [FromQuery] Guid userId)
        => await sender.Send(new MakeSubscriptionPurchaseCommand(dto, userId));
}

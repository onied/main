using Microsoft.AspNetCore.Mvc;
using Purchases.Dtos.Requests;
using Purchases.Services.Abstractions;

namespace Purchases.Controllers;

[Route("api/v1/[controller]")]
public class PurchasesController(
    IPurchaseService purchaseService) : ControllerBase
{
    [HttpGet]
    public async Task<IResult> GetPurchasesByUser(Guid userId)
        => await purchaseService.GetPurchasesByUser(userId);

    [HttpPost("verify")]
    public async Task<IResult> Verify([FromBody] VerifyTokenRequestDto dto)
        => await purchaseService.Verify(dto);
}

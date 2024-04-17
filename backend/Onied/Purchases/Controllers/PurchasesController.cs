using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Purchases.Data.Abstractions;
using Purchases.Dtos.Requests;
using Purchases.Dtos.Responses;
using Purchases.Services.Abstractions;

namespace Purchases.Controllers;

[Route("api/v1/[controller]")]
public class PurchasesController(
    IMapper mapper,
    IUserRepository userRepository,
    IPurchaseRepository purchaseRepository,
    IPurchaseTokenService tokenService) : ControllerBase
{
    [HttpGet]
    public async Task<IResult> GetPurchasesByUser(Guid userId)
    {
        var user = await userRepository.GetAsync(userId, true);
        if (user is null) return Results.NotFound();

        var pInfo = mapper.Map<List<PurchaseInfoResponseDto>>(user.Purchases);
        return Results.Ok(pInfo);
    }

    [HttpPost("verify")]
    public async Task<IResult> Verify([FromBody] VerifyTokenRequestDto dto)
    {
        var purchaseTokenInfo = tokenService.ConvertToPurchaseTokenInfo(dto.Token);

        var user = await userRepository.GetAsync(purchaseTokenInfo.UserId, true);
        var purchase = await purchaseRepository.GetAsync(purchaseTokenInfo.Id);
        if (user is null || purchase is null) return Results.BadRequest();

        return purchase.Token!.Equals(dto.Token) ? Results.Ok() : Results.Forbid();
    }
}

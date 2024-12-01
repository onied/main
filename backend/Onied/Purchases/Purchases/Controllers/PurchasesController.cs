using MediatR;
using Microsoft.AspNetCore.Mvc;
using Purchases.Commands;
using Purchases.Dtos.Requests;
using Purchases.Queries;

namespace Purchases.Controllers;

[Route("api/v1/[controller]")]
public class PurchasesController(
    ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<IResult> GetPurchasesByUser(Guid userId)
        => await sender.Send(new GetPurchasesByUserQuery(userId));

    [HttpPost("verify")]
    public async Task<IResult> Verify([FromBody] VerifyTokenRequestDto dto)
        => await sender.Send(new VerifyCommand(dto));
}

using Microsoft.AspNetCore.Mvc;
using Purchases.Dtos.Requests;
using Purchases.Services.Abstractions;

namespace Purchases.Controllers;

[Route("api/v1/purchases/[controller]")]
public class SubscriptionsController(
    ISubscriptionManagementService subscriptionManagementService
    ) : ControllerBase
{
    [HttpGet]
    public async Task<IResult> GetSubscriptionsByUser(Guid userId)
        => await subscriptionManagementService.GetSubscriptionsByUser(userId);

    [HttpPatch]
    public async Task<IResult> UpdateAutoRenewal(Guid userId, [FromBody] AutoRenewalRequestDto requestDto)
        => await subscriptionManagementService.UpdateAutoRenewal(
            userId,
            requestDto.SubscriptionId,
            requestDto.AutoRenewal);
}

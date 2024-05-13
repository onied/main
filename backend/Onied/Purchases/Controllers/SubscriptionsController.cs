using Microsoft.AspNetCore.Mvc;
using Purchases.Services.Abstractions;
using Purchases.Dtos.Requests;

namespace Purchases.Controllers;

[Route("api/v1/purchases/[controller]")]
public class SubscriptionsController(
    ISubscriptionManagementService subscriptionManagementService
) : ControllerBase
{
    [HttpGet("active")]
    public async Task<IResult> GetActiveSubscription([FromQuery] Guid userId)
        => await subscriptionManagementService.GetActiveSubscription(userId);

    [HttpGet]
    public async Task<IResult> GetSubscriptionsByUser(Guid userId)
        => await subscriptionManagementService.GetSubscriptionsByUser(userId);

    [HttpPatch("{subscriptionId}")]
    public async Task<IResult> UpdateAutoRenewal(
        Guid userId,
        int subscriptionId,
        [FromBody] AutoRenewalRequestDto requestDto)
        => await subscriptionManagementService.UpdateAutoRenewal(userId, subscriptionId, requestDto.AutoRenewal);

    [HttpGet]
    [Route("all")]
    public async Task<IResult> GetAllSubscriptions()
    {
        return await subscriptionManagementService.GetAllSubscriptions();
    }
}

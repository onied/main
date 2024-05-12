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

    [HttpPatch("{subscriptionId}")]
    public async Task<IResult> UpdateAutoRenewal(
        Guid userId,
        int subscriptionId,
        [FromBody] AutoRenewalRequestDto requestDto)
        => await subscriptionManagementService.UpdateAutoRenewal(userId, subscriptionId, requestDto.AutoRenewal);
    {
        return await subscriptionManagementService.GetSubscriptionsByUser(userId);
    }

    [HttpGet]
    [Route("all")]
    public async Task<IResult> GetAllSubscriptions()
    {
        return await subscriptionManagementService.GetAllSubscriptions();
    }
}

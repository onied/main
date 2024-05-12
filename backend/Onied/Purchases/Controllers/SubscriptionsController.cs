using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Purchases.Data.Abstractions;
using Purchases.Services.Abstractions;
using Purchases.Dtos;
using Purchases.Dtos.Requests;

namespace Purchases.Controllers;

[Route("api/v1/purchases/[controller]")]
public class SubscriptionsController(
    IMapper mapper,
    IUserRepository userRepository,
    ISubscriptionManagementService subscriptionManagementService
) : ControllerBase
{
    [HttpGet("active")]
    public async Task<IResult> GetActiveSubscription([FromQuery] Guid userId)
    {
        var user = await userRepository.GetAsync(userId, withSubscription: true);

        return user is null
            ? Results.NotFound()
            : Results.Ok(mapper.Map<SubscriptionDto>(user.Subscription));
    }

    [HttpGet]
    public async Task<IResult> GetSubscriptionsByUser(Guid userId)
        => await subscriptionManagementService.GetSubscriptionsByUser(userId);

    [HttpPatch("{subscriptionId}")]
    public async Task<IResult> UpdateAutoRenewal(
        Guid userId,
        int subscriptionId,
        [FromBody] AutoRenewalRequestDto requestDto)
        => await subscriptionManagementService.UpdateAutoRenewal(userId, subscriptionId, requestDto.AutoRenewal);
}

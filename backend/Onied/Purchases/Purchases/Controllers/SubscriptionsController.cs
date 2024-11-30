using MediatR;
using Microsoft.AspNetCore.Mvc;
using Purchases.Commands;
using Purchases.Dtos.Requests;
using Purchases.Queries;

namespace Purchases.Controllers;

[Route("api/v1/purchases/[controller]")]
public class SubscriptionsController(ISender sender) : ControllerBase
{
    [HttpGet("active")]
    public async Task<IResult> GetActiveSubscription([FromQuery] Guid userId)
        => await sender.Send(new GetActiveSubscriptionQuery(userId));

    [HttpGet]
    public async Task<IResult> GetSubscriptionsByUser(Guid userId)
        => await sender.Send(new GetSubscriptionsByUserQuery(userId));

    [HttpPatch("{subscriptionId}")]
    public async Task<IResult> UpdateAutoRenewal(
        Guid userId,
        int subscriptionId,
        [FromBody] AutoRenewalRequestDto requestDto)
        => await sender.Send(new UpdateAutoRenewalCommand(userId, subscriptionId, requestDto));

    [HttpGet]
    [Route("all")]
    public async Task<IResult> GetAllSubscriptions()
        => await sender.Send(new GetAllSubscriptionsQuery());
}

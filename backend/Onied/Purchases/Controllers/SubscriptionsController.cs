using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Purchases.Data.Abstractions;
using Purchases.Dtos;

namespace Purchases.Controllers;

[Route("/api/v1/[controller]")]
public class SubscriptionsController(
    IMapper mapper,
    IUserRepository userRepository) : ControllerBase
{
    [HttpGet("active")]
    public async Task<IResult> GetActiveSubscription([FromQuery] Guid userId)
    {
        var user = await userRepository.GetAsync(userId, withSubscription: true);

        return user is null
            ? Results.NotFound()
            : Results.Ok(mapper.Map<SubscriptionDto>(user.Subscription));
    }
}

using Microsoft.AspNetCore.Mvc;
using Purchases.Services.Abstractions;

namespace Purchases.Controllers;

[Route("api/v1/purchases/[controller]")]
public class SubscriptionsController(
    ISubscriptionManagementService subscriptionManagementService
    ) : ControllerBase
{
    [HttpGet]
    public async Task<IResult> GetSubscriptionsByUser(Guid userId)
    {
        return await subscriptionManagementService.GetSubscriptionsByUser(userId);
    }
}

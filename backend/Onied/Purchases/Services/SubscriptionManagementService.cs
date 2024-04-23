using AutoMapper;
using Purchases.Data.Abstractions;
using Purchases.Data.Models.PurchaseDetails;
using Purchases.Dtos;
using Purchases.Dtos.Responses;
using Purchases.Services.Abstractions;

namespace Purchases.Services;

public class SubscriptionManagementService(
    IUserRepository userRepository,
    ISubscriptionRepository subscriptionRepository,
    IMapper mapper
    ) : ISubscriptionManagementService
{
    public async Task<IResult> GetSubscriptionsByUser(Guid userId)
    {
        var user = await userRepository.GetAsync(userId, true, true);
        if (user is null) return Results.NotFound();

        var purchaseSubscription = user.Purchases
            .Where(p => p.PurchaseDetails.GetType() == typeof(SubscriptionPurchaseDetails))
            .FirstOrDefault(p => ((SubscriptionPurchaseDetails)p.PurchaseDetails).EndDate > DateTime.Today);
        if (purchaseSubscription == default) return Results.Ok();

        var pInfo = mapper.Map<SubscriptionUserDto>(user.Subscription);
        pInfo.EndDate = ((SubscriptionPurchaseDetails)purchaseSubscription
            .PurchaseDetails).EndDate;

        return Results.Ok(pInfo);
    }

    public async Task<IResult> GetAllSubscriptions()
    {
        var subscriptions = await subscriptionRepository.GetAllSubscriptions();

        if (subscriptions.Count == 0)
            return Results.NotFound();

        return Results.Ok(mapper.Map<List<SubscriptionDto>>(subscriptions));
    }
}

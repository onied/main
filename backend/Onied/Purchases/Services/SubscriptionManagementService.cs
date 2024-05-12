using AutoMapper;
using Purchases.Data.Abstractions;
using Purchases.Data.Enums;
using Purchases.Data.Models.PurchaseDetails;
using Purchases.Dtos;
using Purchases.Dtos.Responses;
using Purchases.Services.Abstractions;

namespace Purchases.Services;

public class SubscriptionManagementService(
    IUserRepository userRepository,
    IPurchaseRepository purchaseRepository,
    IMapper mapper
    ) : ISubscriptionManagementService
{
    public async Task<IResult> GetActiveSubscription(Guid userId)
    {
        var user = await userRepository.GetAsync(userId, withSubscription: true);

        return user is null || user.Subscription.Id == (int)SubscriptionType.Free
            ? Results.NotFound()
            : Results.Ok(mapper.Map<SubscriptionUserDto>(user.Subscription));
    }

    public async Task<IResult> GetSubscriptionsByUser(Guid userId)
    {
        var user = await userRepository.GetAsync(userId, true, true);
        if (user is null) return Results.NotFound();

        var purchaseSubscription = user.Purchases
            .Where(p => p.PurchaseDetails.GetType() == typeof(SubscriptionPurchaseDetails))
            .FirstOrDefault(p => ((SubscriptionPurchaseDetails)p.PurchaseDetails).EndDate > DateTime.Today);

        if (purchaseSubscription == default) return Results.Ok();

        var pInfo = mapper.Map<SubscriptionUserDto>(user.Subscription);

        pInfo.Id = purchaseSubscription.Id;
        pInfo.EndDate = ((SubscriptionPurchaseDetails)purchaseSubscription
            .PurchaseDetails).EndDate;
        pInfo.AutoRenewalEnabled = ((SubscriptionPurchaseDetails)purchaseSubscription
            .PurchaseDetails).AutoRenewalEnabled;

        return Results.Ok(pInfo);
    }

    public async Task<IResult> UpdateAutoRenewal(Guid userId, int subscriptionId, bool autoRenewal)
    {
        var user = await userRepository.GetAsync(userId, true, true);
        if (user is null) return Results.NotFound();

        if (await purchaseRepository.UpdateAutoRenewal(userId, subscriptionId, autoRenewal)) return Results.Ok();

        return Results.NotFound();
    }

    public async Task UpdateSubscriptionWithAutoRenewal()
    {
        await purchaseRepository.UpdateSubscriptionWithAutoRenewal();
    }
}

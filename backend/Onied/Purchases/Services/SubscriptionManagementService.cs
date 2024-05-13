using AutoMapper;
using Purchases.Data.Abstractions;
using Purchases.Data.Enums;
using Purchases.Data.Models.PurchaseDetails;
using Purchases.Dtos.Responses;
using Purchases.Services.Abstractions;

namespace Purchases.Services;

public class SubscriptionManagementService(
    IUserRepository userRepository,
    IPurchaseRepository purchaseRepository,
    ISubscriptionRepository subscriptionRepository,
    IMapper mapper
    ) : ISubscriptionManagementService
{
    public async Task<IResult> GetActiveSubscription(Guid userId)
    {
        var user = await userRepository.GetAsync(userId, withSubscription: true, withPurchases: true);
        if (user is null || user.Subscription.Id == (int)SubscriptionType.Free)
            return Results.NotFound();

        var activeSubPurchase = user.Purchases
            .Single(
                p => p.PurchaseDetails.PurchaseType is PurchaseType.Subscription
                     && (p.PurchaseDetails as SubscriptionPurchaseDetails)!.SubscriptionId == user.SubscriptionId
                     && ((SubscriptionPurchaseDetails)p.PurchaseDetails).EndDate > DateTime.Today);
        var pd = (SubscriptionPurchaseDetails)activeSubPurchase.PurchaseDetails;

        var subscriptionDto = mapper.Map<SubscriptionUserDto>(user.Subscription);
        subscriptionDto.AutoRenewalEnabled = pd.AutoRenewalEnabled;
        subscriptionDto.EndDate = pd.EndDate;
        return Results.Ok(subscriptionDto);
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

    public async Task<IResult> GetAllSubscriptions()
    {
        var subscriptions = await subscriptionRepository.GetAllSubscriptions();

        if (subscriptions.Count == 0)
            return Results.NotFound();

        return Results.Ok(mapper.Map<List<SubscriptionDto>>(subscriptions));
    }
}

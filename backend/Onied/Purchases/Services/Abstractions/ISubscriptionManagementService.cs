﻿namespace Purchases.Services.Abstractions;

public interface ISubscriptionManagementService
{
    public Task<IResult> GetActiveSubscription(Guid userId);
    public Task<IResult> GetSubscriptionsByUser(Guid userId);
    public Task<IResult> UpdateAutoRenewal(Guid userId, int subscriptionId, bool autoRenewal);

    public Task UpdateSubscriptionWithAutoRenewal();
    public Task<IResult> GetAllSubscriptions();
}

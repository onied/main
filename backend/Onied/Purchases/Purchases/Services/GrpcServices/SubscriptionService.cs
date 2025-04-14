using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Purchases.Data.Abstractions;
using Purchases.Data.Enums;
using Purchases.Data.Models.PurchaseDetails;
using PurchasesGrpc;

namespace Purchases.Services.GrpcServices;

public class SubscriptionService(
    IUserRepository userRepository,
    IMapper mapper) : PurchasesGrpc.SubscriptionService.SubscriptionServiceBase
{
    public override async Task<GetActiveSubscriptionReply> GetActiveSubscription(GetActiveSubscriptionRequest request,
        ServerCallContext context)
    {
        var guidBytes = request.UserId.ToByteArray();
        var user = await userRepository.GetAsync(new Guid(guidBytes), withSubscription: true,
            withPurchases: true);
        if (user is null || user.Subscription.Id == (int)SubscriptionType.Free)
            throw new RpcException(new Status(StatusCode.NotFound, "User or paid subscription is not found"));

        var activeSubPurchase = user.Purchases
            .Single(
                p => p.PurchaseDetails.PurchaseType is PurchaseType.Subscription
                     && (p.PurchaseDetails as SubscriptionPurchaseDetails)!.SubscriptionId == user.SubscriptionId
                     && ((SubscriptionPurchaseDetails)p.PurchaseDetails).EndDate > DateTime.Today);
        var purchaseDetails = (SubscriptionPurchaseDetails)activeSubPurchase.PurchaseDetails;

        var getActiveSubscriptionReply = mapper.Map<GetActiveSubscriptionReply>(user.Subscription);
        getActiveSubscriptionReply.AutoRenewalEnabled = purchaseDetails.AutoRenewalEnabled;
        getActiveSubscriptionReply.EndDate = Timestamp.FromDateTime(purchaseDetails.EndDate);

        return getActiveSubscriptionReply;
    }
}

using MediatR;
using Purchases.Queries;
using Purchases.Services.Abstractions;

namespace Purchases.Handlers;

public class GetSubscriptionPreparedPurchaseQueryHandler(
    IPurchaseMakingService purchaseMakingService) : IRequestHandler<GetSubscriptionPreparedPurchaseQuery, IResult>
{
    public async Task<IResult> Handle(
        GetSubscriptionPreparedPurchaseQuery request,
        CancellationToken cancellationToken)
        => await purchaseMakingService.GetSubscriptionPreparedPurchase(request.SubscriptionId);
}

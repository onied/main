using MediatR;
using Purchases.Queries;
using Purchases.Services.Abstractions;

namespace Purchases.Handlers;

public class GetAllSubscriptionsQueryHandler(
    ISubscriptionManagementService subscriptionManagementService)
    : IRequestHandler<GetAllSubscriptionsQuery, IResult>
{
    public async Task<IResult> Handle(
        GetAllSubscriptionsQuery _,
        CancellationToken cancellationToken)
        => await subscriptionManagementService.GetAllSubscriptions();
}

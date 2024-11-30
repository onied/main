using MediatR;
using Purchases.Queries;
using Purchases.Services.Abstractions;

namespace Purchases.Handlers;

public class GetActiveSubscriptionQueryHandler(
    ISubscriptionManagementService subscriptionManagementService)
    : IRequestHandler<GetActiveSubscriptionQuery, IResult>
{
    public async Task<IResult> Handle(
        GetActiveSubscriptionQuery request,
        CancellationToken cancellationToken)
        => await subscriptionManagementService.GetActiveSubscription(request.UserId);
}

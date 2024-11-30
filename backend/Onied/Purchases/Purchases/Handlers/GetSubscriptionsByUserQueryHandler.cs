using MediatR;
using Purchases.Queries;
using Purchases.Services.Abstractions;

namespace Purchases.Handlers;

public class GetSubscriptionsByUserQueryHandler(
    ISubscriptionManagementService subscriptionManagementService)
    : IRequestHandler<GetSubscriptionsByUserQuery, IResult>
{
    public async Task<IResult> Handle(
        GetSubscriptionsByUserQuery request,
        CancellationToken cancellationToken)
        => await subscriptionManagementService.GetSubscriptionsByUser(request.UserId);
}

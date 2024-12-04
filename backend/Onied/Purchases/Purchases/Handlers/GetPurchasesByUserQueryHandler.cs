using MediatR;
using Purchases.Queries;
using Purchases.Services.Abstractions;

namespace Purchases.Handlers;

public class GetPurchasesByUserQueryHandler(
    IPurchaseService purchaseService)
    : IRequestHandler<GetPurchasesByUserQuery, IResult>
{
    public async Task<IResult> Handle(
        GetPurchasesByUserQuery request,
        CancellationToken cancellationToken)
        => await purchaseService.GetPurchasesByUser(request.UserId);
}

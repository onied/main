using MediatR;
using Purchases.Queries;
using Purchases.Services.Abstractions;

namespace Purchases.Handlers;

public class GetCertificatePreparedPurchaseQueryHandler(
    IPurchaseMakingService purchaseMakingService)
    : IRequestHandler<GetCertificatePreparedPurchaseQuery, IResult>
{
    public async Task<IResult> Handle(
        GetCertificatePreparedPurchaseQuery request,
        CancellationToken cancellationToken)
        => await purchaseMakingService.GetCertificatePreparedPurchase(request.CourseId);
}

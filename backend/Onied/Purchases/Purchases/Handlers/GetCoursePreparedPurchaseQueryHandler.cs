using MediatR;
using Purchases.Queries;
using Purchases.Services.Abstractions;

namespace Purchases.Handlers;

public class GetCoursePreparedPurchaseQueryHandler(
    IPurchaseMakingService purchaseMakingService)
    : IRequestHandler<GetCoursePreparedPurchaseQuery, IResult>
{
    public async Task<IResult> Handle(
        GetCoursePreparedPurchaseQuery request,
        CancellationToken cancellationToken)
        => await purchaseMakingService.GetCoursePreparedPurchase(request.CourseId);
}

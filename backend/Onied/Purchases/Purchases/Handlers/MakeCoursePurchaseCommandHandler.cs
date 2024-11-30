using MediatR;
using Purchases.Commands;
using Purchases.Services.Abstractions;

namespace Purchases.Handlers;

public class MakeCoursePurchaseCommandHandler(
    IPurchaseMakingService purchaseMakingService)
    : IRequestHandler<MakeCoursePurchaseCommand, IResult>
{
    public async Task<IResult> Handle(
        MakeCoursePurchaseCommand request,
        CancellationToken cancellationToken)
        => await purchaseMakingService.MakeCoursePurchase(request.Dto, request.UserId);
}

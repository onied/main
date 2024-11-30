using MediatR;
using Purchases.Commands;
using Purchases.Services.Abstractions;

namespace Purchases.Handlers;

public class MakeSubscriptionPurchaseCommandHandler(
    IPurchaseMakingService purchaseMakingService)
    : IRequestHandler<MakeSubscriptionPurchaseCommand, IResult>
{
    public async Task<IResult> Handle(
        MakeSubscriptionPurchaseCommand request,
        CancellationToken cancellationToken)
        => await purchaseMakingService.MakeSubscriptionPurchase(request.Dto, request.UserId);
}

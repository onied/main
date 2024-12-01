using MediatR;
using Purchases.Commands;
using Purchases.Services.Abstractions;

namespace Purchases.Handlers;

public class MakeCertificatePurchaseCommandHandler(
    IPurchaseMakingService purchaseMakingService)
    : IRequestHandler<MakeCertificatePurchaseCommand, IResult>
{
    public async Task<IResult> Handle(
        MakeCertificatePurchaseCommand request,
        CancellationToken cancellationToken)
        => await purchaseMakingService.MakeCertificatePurchase(request.Dto, request.UserId);
}

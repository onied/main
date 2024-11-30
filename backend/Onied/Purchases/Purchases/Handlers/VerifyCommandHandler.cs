using MediatR;
using Purchases.Commands;
using Purchases.Services.Abstractions;

namespace Purchases.Handlers;

public class VerifyCommandHandler(
    IPurchaseService purchaseService)
    : IRequestHandler<VerifyCommand, IResult>
{
    public async Task<IResult> Handle(
        VerifyCommand request,
        CancellationToken cancellationToken)
        => await purchaseService.Verify(request.Dto);
}

using MediatR;
using Purchases.Commands;
using Purchases.Services.Abstractions;

namespace Purchases.Handlers;

public class UpdateAutoRenewalCommandHandler(
    ISubscriptionManagementService subscriptionManagementService)
    : IRequestHandler<UpdateAutoRenewalCommand, IResult>
{
    public async Task<IResult> Handle(
        UpdateAutoRenewalCommand request,
        CancellationToken cancellationToken)
        => await subscriptionManagementService.UpdateAutoRenewal(
            request.UserId,
            request.SubscriptionId,
            request.RequestDto.AutoRenewal);
}

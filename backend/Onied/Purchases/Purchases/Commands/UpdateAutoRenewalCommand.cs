using MediatR;
using Purchases.Dtos.Requests;

namespace Purchases.Commands;

public record UpdateAutoRenewalCommand(
    Guid UserId,
    int SubscriptionId,
    AutoRenewalRequestDto RequestDto) : IRequest<IResult>;

using MediatR;
using Purchases.Dtos.Requests;

namespace Purchases.Commands;

public record MakeSubscriptionPurchaseCommand(PurchaseRequestDto Dto, Guid UserId) : IRequest<IResult>;
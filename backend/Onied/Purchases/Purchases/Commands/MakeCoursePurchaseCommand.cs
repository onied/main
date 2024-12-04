using MediatR;
using Purchases.Dtos.Requests;

namespace Purchases.Commands;

public record MakeCoursePurchaseCommand(PurchaseRequestDto Dto, Guid UserId) : IRequest<IResult>;

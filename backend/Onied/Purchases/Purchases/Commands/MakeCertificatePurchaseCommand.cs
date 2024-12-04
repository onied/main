using MediatR;
using Purchases.Dtos.Requests;

namespace Purchases.Commands;

public record MakeCertificatePurchaseCommand(PurchaseRequestDto Dto, Guid UserId) : IRequest<IResult>;

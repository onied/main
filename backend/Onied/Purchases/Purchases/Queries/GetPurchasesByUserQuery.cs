using MediatR;

namespace Purchases.Queries;

public record GetPurchasesByUserQuery(Guid UserId) : IRequest<IResult>;

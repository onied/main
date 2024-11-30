using MediatR;

namespace Purchases.Queries;

public record GetPurchasesByUserQuery(Guid userId) : IRequest<IResult>;

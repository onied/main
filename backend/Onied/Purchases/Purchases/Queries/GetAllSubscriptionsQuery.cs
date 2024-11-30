using MediatR;

namespace Purchases.Queries;

public record GetAllSubscriptionsQuery : IRequest<IResult>;

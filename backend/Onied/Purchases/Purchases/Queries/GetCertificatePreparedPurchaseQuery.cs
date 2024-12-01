using MediatR;

namespace Purchases.Queries;

public record GetCertificatePreparedPurchaseQuery(int CourseId) : IRequest<IResult>;

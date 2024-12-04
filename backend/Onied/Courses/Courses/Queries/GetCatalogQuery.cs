using Courses.Dtos.Catalog.Request;
using MediatR;

namespace Courses.Queries;

public record GetCatalogQuery(Guid? UserId, CatalogGetQueriesRequest CatalogGetQueriesRequest)
    : IRequest<IResult>;

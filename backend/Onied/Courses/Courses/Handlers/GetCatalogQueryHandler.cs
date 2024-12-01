using Courses.Queries;
using Courses.Services.Abstractions;
using MediatR;

namespace Courses.Handlers;

public class GetCatalogQueryHandler(ICatalogService catalogService)
    : IRequestHandler<GetCatalogQuery, IResult>
{
    public async Task<IResult> Handle(GetCatalogQuery request, CancellationToken cancellationToken)
    {
        return await catalogService.Get(request.CatalogGetQueriesRequest, request.UserId);
    }
}

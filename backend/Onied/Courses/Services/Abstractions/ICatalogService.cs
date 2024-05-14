using Courses.Dtos;
using Courses.Dtos.Catalog.Request;

namespace Courses.Services.Abstractions;

public interface ICatalogService
{
    public Task<IResult> Get(
        CatalogGetQueriesRequest catalogGetQueries,
        Guid? userId);
}

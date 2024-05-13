using Courses.Dtos;

namespace Courses.Services.Abstractions;

public interface ICatalogService
{
    public Task<IResult> Get(
        CatalogGetQueriesDto catalogGetQueries,
        Guid? userId);
}

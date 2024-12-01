using Courses.Dtos.Catalog.Request;
using Courses.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Courses.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class CatalogController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<IResult> Get(
        [FromQuery] CatalogGetQueriesRequest catalogGetQueries,
        [FromQuery] Guid? userId
    )
    {
        return await sender.Send(new GetCatalogQuery(userId, catalogGetQueries));
    }
}

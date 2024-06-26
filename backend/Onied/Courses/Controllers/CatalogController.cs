using Courses.Dtos.Catalog.Request;
using Courses.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Courses.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class CatalogController(ICatalogService catalogService)
    : ControllerBase
{
    [HttpGet]
    public async Task<IResult> Get(
        [FromQuery] CatalogGetQueriesRequest catalogGetQueries,
        [FromQuery] Guid? userId)
    {
        return await catalogService.Get(catalogGetQueries, userId);
    }
}

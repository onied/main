using AutoMapper;
using Courses.Dtos;
using Courses.Helpers;
using Courses.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Courses.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class CatalogController(
    ICatalogService catalogService,
    ILogger<CatalogController> logger)
    : ControllerBase
{
    [HttpGet]
    public async Task<IResult> Get(
        [FromQuery] CatalogGetQueriesDto catalogGetQueries,
        [FromQuery] Guid? userId)
    {
        return await catalogService.Get(catalogGetQueries, userId);
    }
}

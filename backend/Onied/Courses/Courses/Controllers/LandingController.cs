using Courses.Dtos.Catalog.Response;
using Courses.Queries;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Courses.Controllers;

[Route("api/v1/[controller]")]
public class LandingController(ISender sender) : ControllerBase
{
    [HttpGet]
    [Route("most-popular")]
    public async Task<Results<Ok<List<CourseCardResponse>>, NotFound>> GetMostPopular()
    {
        return await sender.Send(new GetMostPopularCoursesQuery(50));
    }

    [HttpGet]
    [Route("recommended")]
    public async Task<Results<Ok<List<CourseCardResponse>>, NotFound>> GetRecommended()
    {
        return await sender.Send(new GetRecommendedCoursesQuery(50));
    }
}

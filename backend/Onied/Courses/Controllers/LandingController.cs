using Courses.Dtos;
using Courses.Services.Abstractions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Courses.Controllers;

[Route("api/v1/[controller]")]
public class LandingController(ILandingPageContentService landingPageContentService) : ControllerBase
{
    [HttpGet]
    [Route("most-popular")]
    public async Task<Results<Ok<List<CourseCardDto>>, NotFound>> GetMostPopular()
    {
        return await landingPageContentService.GetMostPopularCourses(50);
    }

    [HttpGet]
    [Route("recommended")]
    public async Task<Results<Ok<List<CourseCardDto>>, NotFound>> GetRecommended()
    {
        return await landingPageContentService.GetRecommendedCourses(50);
    }
}

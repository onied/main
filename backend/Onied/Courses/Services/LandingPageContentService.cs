using AutoMapper;
using Courses.Dtos;
using Courses.Dtos.Catalog.Response;
using Courses.Services.Abstractions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Courses.Services;

public class LandingPageContentService(
    ICourseRepository courseRepository,
    IMapper mapper) : ILandingPageContentService
{
    public async Task<Results<Ok<List<CourseCardResponse>>, NotFound>> GetMostPopularCourses(int amount)
    {
        var result = await courseRepository.GetMostPopularCourses(amount);
        return result.Count == 0
            ? TypedResults.NotFound()
            : TypedResults.Ok(mapper.Map<List<CourseCardResponse>>(result));
    }

    public async Task<Results<Ok<List<CourseCardResponse>>, NotFound>> GetRecommendedCourses(int amount)
    {
        var result = await courseRepository.GetRecommendedCourses(amount);
        return result.Count == 0
            ? TypedResults.NotFound()
            : TypedResults.Ok(mapper.Map<List<CourseCardResponse>>(result));
    }
}

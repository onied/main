using Courses.Dtos;
using Courses.Dtos.Catalog.Response;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Courses.Services.Abstractions;

public interface ILandingPageContentService
{
    public Task<Results<Ok<List<CourseCardResponse>>, NotFound>> GetMostPopularCourses(int amount);
    public Task<Results<Ok<List<CourseCardResponse>>, NotFound>> GetRecommendedCourses(int amount);
}

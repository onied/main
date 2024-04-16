using Courses.Dtos;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Courses.Services.Abstractions;

public interface ILandingPageContentService
{
    public Task<Results<Ok<List<CourseCardDto>>, NotFound>> GetMostPopularCourses(int amount);
    public Task<Results<Ok<List<CourseCardDto>>, NotFound>> GetRecommendedCourses(int amount);
}

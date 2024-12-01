using Courses.Dtos.Catalog.Response;
using Courses.Queries;
using Courses.Services.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Courses.Handlers;

public class GetRecommendedCoursesQueryHandler(ILandingPageContentService landingPageContentService)
    : IRequestHandler<GetRecommendedCoursesQuery, Results<Ok<List<CourseCardResponse>>, NotFound>>
{
    public async Task<Results<Ok<List<CourseCardResponse>>, NotFound>> Handle(
        GetRecommendedCoursesQuery request,
        CancellationToken cancellationToken
    )
    {
        return await landingPageContentService.GetRecommendedCourses(request.CoursesAmount);
    }
}

using Courses.Dtos.Catalog.Response;
using Courses.Queries;
using Courses.Services.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Courses.Handlers;

public class GetMostPopularCoursesHandler(ILandingPageContentService landingPageContentService)
    : IRequestHandler<GetMostPopularCoursesQuery, Results<Ok<List<CourseCardResponse>>, NotFound>>
{
    public async Task<Results<Ok<List<CourseCardResponse>>, NotFound>> Handle(
        GetMostPopularCoursesQuery request,
        CancellationToken cancellationToken
    )
    {
        return await landingPageContentService.GetMostPopularCourses(request.CoursesAmount);
    }
}

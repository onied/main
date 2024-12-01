using Courses.Queries;
using Courses.Services.Abstractions;
using MediatR;

namespace Courses.Handlers;

public class GetCourseHierarchyQueryHandler(ICourseService courseService) : IRequestHandler<GetCourseHierarchyQuery, IResult>
{
    public async Task<IResult> Handle(GetCourseHierarchyQuery request, CancellationToken cancellationToken)
    {
        return await courseService.GetCourseHierarchy(request.Id, request.UserId, request.Role);
    }
}

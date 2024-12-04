using Courses.Queries;
using Courses.Services.Abstractions;
using MediatR;

namespace Courses.Handlers;

public class CheckEditCourseQueryHandler(ICourseManagementService courseManagementService)
    : IRequestHandler<CheckEditCourseQuery, IResult>
{
    public async Task<IResult> Handle(CheckEditCourseQuery request, CancellationToken cancellationToken)
    {
        return await courseManagementService.CheckCourseAuthorAsync(request.Id, request.UserId, request.Role);
    }
}

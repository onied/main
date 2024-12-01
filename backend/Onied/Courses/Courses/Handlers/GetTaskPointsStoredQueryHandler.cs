using Courses.Queries;
using Courses.Services.Abstractions;
using MediatR;

namespace Courses.Handlers;

public class GetTaskPointsStoredQueryHandler(ICheckTaskManagementService checkTaskManagementService)
    : IRequestHandler<GetTaskPointsStoredQuery, IResult>
{
    public async Task<IResult> Handle(
        GetTaskPointsStoredQuery request,
        CancellationToken cancellationToken
    )
    {
        return await checkTaskManagementService.GetTaskPointsStored(
            request.CourseId,
            request.BlockId,
            request.UserId,
            request.Role
        );
    }
}

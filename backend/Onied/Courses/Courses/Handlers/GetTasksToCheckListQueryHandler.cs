using Courses.Queries;
using Courses.Services.Abstractions;
using MediatR;

namespace Courses.Handlers;

public class GetTasksToCheckListQueryHandler(IManualReviewService manualReviewService)
    : IRequestHandler<GetTasksToCheckListQuery, IResult>
{
    public async Task<IResult> Handle(
        GetTasksToCheckListQuery request,
        CancellationToken cancellationToken
    )
    {
        return await manualReviewService.GetTasksToCheckForTeacher(request.UserId);
    }
}

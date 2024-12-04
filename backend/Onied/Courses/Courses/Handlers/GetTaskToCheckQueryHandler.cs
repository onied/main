using Courses.Queries;
using Courses.Services.Abstractions;
using MediatR;

namespace Courses.Handlers;

public class GetTaskToCheckQueryHandler(IManualReviewService manualReviewService)
    : IRequestHandler<GetTaskToCheckQuery, IResult>
{
    public async Task<IResult> Handle(
        GetTaskToCheckQuery request,
        CancellationToken cancellationToken
    )
    {
        return await manualReviewService.GetManualReviewTaskUserAnswer(
            request.UserId,
            request.UserAnswerId
        );
    }
}

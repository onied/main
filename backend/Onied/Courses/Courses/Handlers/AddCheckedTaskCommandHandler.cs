using Courses.Commands;
using Courses.Services.Abstractions;
using MediatR;

namespace Courses.Handlers;

public class AddCheckedTaskCommandHandler(IManualReviewService manualReviewService)
    : IRequestHandler<AddCheckedTaskCommand, IResult>
{
    public async Task<IResult> Handle(
        AddCheckedTaskCommand request,
        CancellationToken cancellationToken
    )
    {
        return await manualReviewService.ReviewUserAnswer(
            request.UserId,
            request.UserAnswerId,
            request.ReviewTaskRequest
        );
    }
}

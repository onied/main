using Courses.Dtos.ManualReview.Request;
using MediatR;

namespace Courses.Commands;

public record AddCheckedTaskCommand(
    Guid UserId,
    Guid UserAnswerId,
    ReviewTaskRequest ReviewTaskRequest
) : IRequest<IResult>;

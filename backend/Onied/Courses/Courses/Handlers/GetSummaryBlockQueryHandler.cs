using Courses.Queries;
using Courses.Services.Abstractions;
using MediatR;

namespace Courses.Handlers;

public class GetSummaryBlockQueryHandler(ICourseService courseService) : IRequestHandler<GetSummaryBlockQuery, IResult>
{
    public async Task<IResult> Handle(GetSummaryBlockQuery request, CancellationToken cancellationToken)
    {
        return await courseService.GetSummaryBlock(request.Id, request.BlockId, request.UserId, request.Role);
    }
}

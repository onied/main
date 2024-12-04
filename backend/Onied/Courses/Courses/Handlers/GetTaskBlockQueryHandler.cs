using Courses.Queries;
using Courses.Services.Abstractions;
using MediatR;

namespace Courses.Handlers;

public class GetTaskBlockQueryHandler(ICourseService courseService) : IRequestHandler<GetTaskBlockQuery, IResult>
{
    public async Task<IResult> Handle(GetTaskBlockQuery request, CancellationToken cancellationToken)
    {
        return await courseService.GetTaskBlock(request.Id, request.BlockId, request.UserId, request.Role);
    }
}

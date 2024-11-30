using Courses.Queries;
using Courses.Services.Abstractions;
using MediatR;

namespace Courses.Handlers;

public class GetEditTaskBlockQueryHandler(ICourseService courseService) : IRequestHandler<GetEditTaskBlockQuery, IResult>
{
    public async Task<IResult> Handle(GetEditTaskBlockQuery request, CancellationToken cancellationToken)
    {
        return await courseService.GetEditTaskBlock(request.Id, request.BlockId, request.UserId, request.Role);
    }
}

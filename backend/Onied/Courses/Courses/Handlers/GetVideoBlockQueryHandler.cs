using Courses.Queries;
using Courses.Services.Abstractions;
using MediatR;

namespace Courses.Handlers;

public class GetVideoBlockQueryHandler(ICourseService courseService) : IRequestHandler<GetVideoBlockQuery, IResult>
{
    public async Task<IResult> Handle(GetVideoBlockQuery request, CancellationToken cancellationToken)
    {
        return await courseService.GetVideoBlock(request.Id, request.BlockId, request.UserId, request.Role);
    }
}

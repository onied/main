using Courses.Commands;
using Courses.Services.Abstractions;
using MediatR;

namespace Courses.Handlers;

public class LikeCourseHandler(ICourseService service) : IRequestHandler<LikeCourseCommand, IResult>
{
    public async Task<IResult> Handle(LikeCourseCommand request, CancellationToken cancellationToken)
    {
        return await service.LikeCourse(request.Id, request.UserId, request.IsLike);
    }
}

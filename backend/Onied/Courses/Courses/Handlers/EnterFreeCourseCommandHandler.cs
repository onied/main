using Courses.Commands;
using Courses.Services.Abstractions;
using MediatR;

namespace Courses.Handlers;

public class EnterFreeCourseCommandHandler(ICourseService courseService) : IRequestHandler<EnterFreeCourseCommand, IResult>
{
    public async Task<IResult> Handle(EnterFreeCourseCommand request, CancellationToken cancellationToken)
    {
        return await courseService.EnterFreeCourse(request.Id, request.UserId);
    }
}

using Courses.Commands;
using Courses.Services.Abstractions;
using MediatR;

namespace Courses.Handlers;

public class CreateCourseCommandHandler(ICourseService courseService) : IRequestHandler<CreateCourseCommand, IResult>
{
    public async Task<IResult> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
    {
        return await courseService.CreateCourse(request.UserId);
    }
}

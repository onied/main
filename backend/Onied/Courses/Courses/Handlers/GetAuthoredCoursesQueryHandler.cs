using Courses.Queries;
using Courses.Services.Abstractions;
using MediatR;

namespace Courses.Handlers;

public class GetAuthoredCoursesQueryHandler(ITeachingService teachingService)
    : IRequestHandler<GetAuthoredCoursesQuery, IResult>
{
    public async Task<IResult> Handle(
        GetAuthoredCoursesQuery request,
        CancellationToken cancellationToken
    )
    {
        return await teachingService.GetAuthoredCourses(request.UserId);
    }
}

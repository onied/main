using Courses.Queries;
using Courses.Services.Abstractions;
using MediatR;

namespace Courses.Handlers;

public class GetModeratedCoursesQueryHandler(ITeachingService teachingService)
    : IRequestHandler<GetModeratedCoursesQuery, IResult>
{
    public async Task<IResult> Handle(
        GetModeratedCoursesQuery request,
        CancellationToken cancellationToken
    )
    {
        return await teachingService.GetModeratedCourses(request.UserId);
    }
}

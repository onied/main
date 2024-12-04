using Courses.Queries;
using Courses.Services.Abstractions;
using MediatR;

namespace Courses.Handlers;

public class GetStudentsQueryHandler(ICourseManagementService courseManagementService)
    : IRequestHandler<GetStudentsQuery, IResult>
{
    public async Task<IResult> Handle(GetStudentsQuery request, CancellationToken cancellationToken)
    {
        return await courseManagementService.GetStudents(request.CourseId, request.UserId);
    }
}

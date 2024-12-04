using Courses.Queries;
using Courses.Services.Abstractions;
using MediatR;

namespace Courses.Handlers;

public class GetCoursesQueryHandler(IAccountsService accountsService)
    : IRequestHandler<GetCoursesQuery, IResult>
{
    public async Task<IResult> Handle(GetCoursesQuery request, CancellationToken cancellationToken)
    {
        return await accountsService.GetCourses(request.UserId);
    }
}

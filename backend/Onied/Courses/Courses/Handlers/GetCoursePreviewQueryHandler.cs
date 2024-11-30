using Courses.Queries;
using Courses.Services.Abstractions;
using MediatR;

namespace Courses.Handlers;

public class GetCoursePreviewQueryHandler(ICourseService courseService) : IRequestHandler<GetCoursePreviewQuery, IResult>
{
    public async Task<IResult> Handle(GetCoursePreviewQuery request, CancellationToken cancellationToken)
    {
        return await courseService.GetCoursePreview(request.Id, request.UserId);
    }
}

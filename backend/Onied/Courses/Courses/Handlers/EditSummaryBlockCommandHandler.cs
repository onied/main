using Courses.Commands;
using Courses.Services.Abstractions;
using MediatR;

namespace Courses.Handlers;

public class EditSummaryBlockCommandHandler(ICourseManagementService courseManagementService)
    : IRequestHandler<EditSummaryBlockCommand, IResult>
{
    public async Task<IResult> Handle(EditSummaryBlockCommand request, CancellationToken cancellationToken)
    {
        return await courseManagementService.EditSummaryBlock(request.Id, request.BlockId,
            request.SummaryBlockResponse);
    }
}

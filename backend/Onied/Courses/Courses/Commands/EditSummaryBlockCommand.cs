using Courses.Dtos.Course.Response;
using MediatR;

namespace Courses.Commands;

public record EditSummaryBlockCommand(int Id, int BlockId, SummaryBlockResponse SummaryBlockResponse) : IRequest<IResult>;

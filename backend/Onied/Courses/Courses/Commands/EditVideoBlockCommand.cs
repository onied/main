using Courses.Dtos.Course.Response;
using MediatR;

namespace Courses.Commands;

public record EditVideoBlockCommand(int Id, int BlockId, VideoBlockResponse VideoBlockResponse) : IRequest<IResult>;

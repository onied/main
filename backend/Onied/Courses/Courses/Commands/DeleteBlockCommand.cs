using MediatR;

namespace Courses.Commands;

public record DeleteBlockCommand(int Id, int BlockId) : IRequest<IResult>;

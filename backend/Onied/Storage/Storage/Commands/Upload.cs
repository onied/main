using MediatR;

namespace Storage.Commands;

public record Upload(IFormFileCollection Files) : IRequest<IResult>;

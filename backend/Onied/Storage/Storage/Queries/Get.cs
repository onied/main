using MediatR;

namespace Storage.Queries;

public record Get(string ObjectName) : IRequest<IResult>;

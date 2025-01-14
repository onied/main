using MediatR;

namespace Storage.Queries;

public record GetUrlByFileIdQuery(string FileId) : IRequest<IResult>;

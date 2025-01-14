using MediatR;

namespace Storage.Queries;

public record GetMetadataByFileIdQuery(string FileId) : IRequest<IResult>;

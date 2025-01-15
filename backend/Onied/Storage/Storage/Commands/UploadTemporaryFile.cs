using MediatR;

namespace Storage.Commands;

public record UploadTemporaryFile(Guid FileId, IFormFile File) : IRequest<IResult>;

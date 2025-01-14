using MediatR;

namespace Storage.Commands;

public record InitUploadTemporaryFile() : IRequest<IResult>;

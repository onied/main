using System.Text.Json;
using MediatR;

namespace Storage.Commands;

public record UploadMetadata(Guid FileId, JsonElement Metadata) : IRequest<IResult>;

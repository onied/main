using System.Text.Json.Nodes;
using MediatR;

namespace Storage.Commands;

public record UploadMetadata(Guid FileId, JsonObject Metadata) : IRequest<IResult>;

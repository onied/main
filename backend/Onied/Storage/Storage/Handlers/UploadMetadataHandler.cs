using MediatR;
using Storage.Abstractions;
using Storage.Commands;
using Storage.Services;

namespace Storage.Handlers;

public class UploadMetadataHandler(TemporaryStorageService storageService, IRedisRepository repository) : IRequestHandler<UploadMetadata, IResult>
{
    public async Task<IResult> Handle(UploadMetadata request, CancellationToken cancellationToken)
    {
        return await storageService.UploadMetadata(request.FileId, request.Metadata, repository);
    }
}

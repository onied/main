using MediatR;
using Storage.Abstractions;
using Storage.Commands;
using Storage.Services;

namespace Storage.Handlers;

public class UploadTemporaryFileHandler(TemporaryStorageService storageService, IRedisRepository repository) : IRequestHandler<UploadTemporaryFile, IResult>
{
    public async Task<IResult> Handle(UploadTemporaryFile request, CancellationToken cancellationToken)
    {
        return await storageService.UploadFile(request.FileId, request.File, repository);
    }
}

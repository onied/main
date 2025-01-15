using MediatR;
using Storage.Abstractions;
using Storage.Commands;
using Storage.Services;

namespace Storage.Handlers;

public class InitUploadTemporaryFileHandler(TemporaryStorageService storageService, IRedisRepository repository)
    : IRequestHandler<InitUploadTemporaryFile, IResult>
{
    public async Task<IResult> Handle(InitUploadTemporaryFile request, CancellationToken cancellationToken)
    {
        return await storageService.InitUpload(repository);
    }
}

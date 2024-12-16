using MediatR;
using Storage.Abstractions;
using Storage.Commands;

namespace Storage.Handlers;

public class UploadHandler(IStorageService storageService) : IRequestHandler<Upload, IResult>
{
    public async Task<IResult> Handle(Upload request, CancellationToken cancellationToken)
    {
        return await storageService.Upload(request.Files);
    }
}

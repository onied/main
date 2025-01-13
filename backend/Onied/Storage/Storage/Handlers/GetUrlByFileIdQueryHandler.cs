using MediatR;
using Storage.Abstractions;
using Storage.Queries;

namespace Storage.Handlers;

public class GetUrlByFileIdQueryHandler(IStorageService storageService) : IRequestHandler<GetUrlByFileIdQuery, IResult>
{
    public async Task<IResult> Handle(GetMetadataByFileIdQuery request, CancellationToken cancellationToken)
    {
        return await storageService.GetMetadata(request.FileId);
    }

    public async Task<IResult> Handle(GetUrlByFileIdQuery request, CancellationToken cancellationToken)
    {
        return await storageService.GetUrlByFileId(request.FileId);
    }
}

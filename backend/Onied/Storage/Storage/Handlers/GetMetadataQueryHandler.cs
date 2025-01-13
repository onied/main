using MediatR;
using Storage.Abstractions;
using Storage.Queries;

namespace Storage.Handlers;

public class GetMetadataQueryHandler(IStorageService storageService) : IRequestHandler<GetMetadataByFileIdQuery, IResult>
{
    public async Task<IResult> Handle(GetMetadataByFileIdQuery request, CancellationToken cancellationToken)
    {
        return await storageService.GetMetadata(request.FileId);
    }
}

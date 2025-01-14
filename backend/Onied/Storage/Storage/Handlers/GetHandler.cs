using MediatR;
using Storage.Abstractions;
using Storage.Queries;

namespace Storage.Handlers;

public class GetHandler(IStorageService storageService) : IRequestHandler<Get, IResult>
{
    public async Task<IResult> Handle(Get request, CancellationToken cancellationToken)
    {
        return await storageService.Get(request.ObjectName);
    }
}

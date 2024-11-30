using AutoMapper;
using MediatR;
using Notifications.Data.Abstractions;
using Notifications.Dtos;
using Notifications.Queries;

namespace Notifications.Handlers;

public class GetMessagesQueryHandler(
    IMapper mapper,
    INotificationRepository notificationRepository)
    : IRequestHandler<GetMessagesQuery, IResult>
{
    public async Task<IResult> Handle(
        GetMessagesQuery request,
        CancellationToken cancellationToken)
    {
        var notifications =
            await notificationRepository.GetRangeByUserAsync(request.UserId);
        return Results.Ok(mapper.Map<List<NotificationDto>>(notifications));
    }
}

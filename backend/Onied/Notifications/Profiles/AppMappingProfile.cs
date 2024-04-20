using AutoMapper;
using MassTransit.Data.Messages;
using Notifications.Data.Models;
using Notifications.Dtos.Responses;

namespace Notifications.Profiles;

public class AppMappingProfile : Profile
{
    public AppMappingProfile()
    {
        CreateMap<Notification, NotificationResponseDto>()
            .ForMember(dest => dest.Img, opt => opt.MapFrom(src => src.Image));
        CreateMap<NotificationSent, Notification>()
            .ForMember(dest => dest.IsRead, opt => opt.MapFrom(src => false));
    }
}

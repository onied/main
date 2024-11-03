using AutoMapper;
using MassTransit.Data.Messages;
using Notifications.Data.Models;
using Notifications.Dtos;

namespace Notifications.Profiles;

public class AppMappingProfile : Profile
{
    public AppMappingProfile()
    {
        CreateMap<Notification, NotificationDto>()
            .ForMember(dest => dest.Img, opt => opt.MapFrom(src => src.Image));
        CreateMap<NotificationSent, Notification>()
            .ForMember(dest => dest.IsRead, opt => opt.MapFrom(src => false));
    }
}

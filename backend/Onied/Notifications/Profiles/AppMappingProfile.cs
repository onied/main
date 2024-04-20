using AutoMapper;
using Notifications.Data.Models;
using Notifications.Dtos.Responses;

namespace Notifications.Profiles;

public class AppMappingProfile : Profile
{
    public AppMappingProfile()
    {
        CreateMap<Notification, NotificationResponseDto>()
            .ForMember(dest => dest.Img, opt => opt.MapFrom(src => src.Image)).ReverseMap();
    }
}

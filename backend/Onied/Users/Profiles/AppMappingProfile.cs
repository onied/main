using AutoMapper;
using MassTransit.Data.Messages;
using Users.Data;
using Users.Dtos;

namespace Users.Profiles;

public class AppMappingProfile : Profile
{
    public AppMappingProfile()
    {
        CreateMap<AppUser, UserProfileDto>();

        // MassTransit
        CreateMap<AppUser, UserCreated>()
            .ForMember(
                dest => dest.AvatarHref,
                opt => opt.MapFrom(src => src.Avatar));
        CreateMap<AppUser, ProfileUpdated>();
        CreateMap<AppUser, ProfilePhotoUpdated>()
            .ForMember(
                dest => dest.AvatarHref,
                opt => opt.MapFrom(src => src.Avatar));
    }
}

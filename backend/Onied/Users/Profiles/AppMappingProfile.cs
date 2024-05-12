using AutoMapper;
using MassTransit.Data.Messages;
using Users.Data;
using Users.Dtos;
using Users.Dtos.Users.Response;

namespace Users.Profiles;

public class AppMappingProfile : Profile
{
    public AppMappingProfile()
    {
        CreateMap<AppUser, UserProfileResponse>();

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

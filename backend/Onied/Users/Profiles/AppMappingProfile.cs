using AutoMapper;
using Users.Dtos;

namespace Users.Profiles;

public class AppMappingProfile : Profile
{
    public AppMappingProfile()
    {
        CreateMap<AppUser, UserProfileDto>();
    }
}

using AutoMapper;
using Purchases.Data.Models;
using Purchases.Dtos;

namespace Purchases.Profiles;

public class AppMappingProfile : Profile
{
    public AppMappingProfile()
    {
        CreateMap<Course, PreparedPurchaseResponseDto>()
            .ForMember(
                dest => dest.PurchaseType,
                opt => opt.Ignore());
        CreateMap<Subscription, PreparedPurchaseResponseDto>()
            .ForMember(
                dest => dest.PurchaseType,
                opt => opt.Ignore());
        CreateMap<PurchaseRequestDto, Purchase>()
            .ForMember(
                dest => dest.PurchaseDetails,
                opt => opt.Ignore());
    }
}

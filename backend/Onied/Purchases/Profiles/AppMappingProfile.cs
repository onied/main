using AutoMapper;
using MassTransit.Data.Messages;
using Purchases.Data.Models;
using Purchases.Data.Models.PurchaseDetails;
using Purchases.Dtos;
using Purchases.Dtos.Requests;
using Purchases.Dtos.Responses;

namespace Purchases.Profiles;

public class AppMappingProfile : Profile
{
    public AppMappingProfile()
    {
        CreateMap<PurchaseRequestDto, Purchase>()
            .ForMember(
                dest => dest.PurchaseDetails,
                opt => opt.Ignore());

        CreateMap<Course, CourseDto>();
        CreateMap<CoursePurchaseDetails, PurchaseDetailsDto>();

        CreateMap<Subscription, SubscriptionDto>();
        CreateMap<SubscriptionPurchaseDetails, PurchaseDetailsDto>();

        CreateMap<CertificatePurchaseDetails, PurchaseDetailsDto>();

        CreateMap<PurchaseDetails, PurchaseDetailsDto>()
            .Include<CoursePurchaseDetails, PurchaseDetailsDto>()
            .Include<SubscriptionPurchaseDetails, PurchaseDetailsDto>()
            .Include<CertificatePurchaseDetails, PurchaseDetailsDto>();

        CreateMap<Course, PreparedPurchaseResponseDto>()
            .ForMember(
                dest => dest.PurchaseType,
                opt => opt.Ignore());
        CreateMap<Subscription, PreparedPurchaseResponseDto>()
            .ForMember(
                dest => dest.PurchaseType,
                opt => opt.Ignore());

        CreateMap<Purchase, PurchaseInfoResponseDto>();

        CreateMap<UserCreated, User>();
        CreateMap<CourseUpdated, Course>();
        CreateMap<CourseCreated, Course>();
    }
}

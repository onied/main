using AutoMapper;
using MassTransit.Data.Messages;
using Purchases.Data.Models;
using Purchases.Data.Models.PurchaseDetails;
using Purchases.Dtos;
using Purchases.Dtos.Requests;
using Purchases.Dtos.Responses;
using PurchasesGrpc;

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

        CreateMap<Subscription, SubscriptionDto>()
            .ForMember(
                dest => dest.CourseCreatingEnabled,
                opt => opt.MapFrom(src => src.ActiveCoursesNumber != 0));
        CreateMap<Subscription, SubscriptionUserDto>()
            .ForMember(
                dest => dest.CourseCreatingEnabled,
                opt => opt.MapFrom(src => src.ActiveCoursesNumber != 0));

        CreateMap<Subscription, GetActiveSubscriptionReply>()
            .ForMember(dest => dest.CourseCreatingEnabled,
                opt => opt.MapFrom(src => src.ActiveCoursesNumber != 0))
            .ForMember(dest => dest.Price,
                opt => opt.MapFrom(src => (DecimalValue.DecimalValue)src.Price));

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
        CreateMap<CourseCompleted, UserCourseInfo>().ForMember(
            dest => dest.IsCompleted,
            opt => opt.MapFrom(src => true));

        CreateMap<Subscription, SubscriptionChanged>();
    }
}

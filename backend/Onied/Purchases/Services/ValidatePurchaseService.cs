using Purchases.Data.Abstractions;
using Purchases.Data.Enums;
using Purchases.Data.Models.PurchaseDetails;
using Purchases.Dtos.Requests;
using Purchases.Services.Abstractions;

namespace Purchases.Services;

public class ValidatePurchaseService(
    IUserRepository userRepository,
    ICourseRepository courseRepository,
    ISubscriptionRepository subscriptionRepository,
    IUserCourseInfoRepository userCourseInfoRepository) : IValidatePurchaseService
{
    public async Task<IResult?> ValidatePurchase(PurchaseRequestDto dto, PurchaseType purchaseType)
    {
        return dto.PurchaseType switch
        {
            PurchaseType.Course => await ValidateCoursePurchase(dto),
            PurchaseType.Certificate => await ValidateCertificatePurchase(dto),
            PurchaseType.Subscription => await ValidateSubscriptionPurchase(dto),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private async Task<IResult?> ValidateCoursePurchase(PurchaseRequestDto dto)
    {
        if (dto.CourseId is null) return Results.BadRequest();

        var user = await userRepository.GetAsync(dto.UserId!.Value, true);
        var course = await courseRepository.GetAsync(dto.CourseId!.Value);
        if (user is null || course is null) return Results.NotFound(); // validation service

        var maybeAlreadyBought = user.Purchases
            .SingleOrDefault(p => p.PurchaseDetails.PurchaseType is PurchaseType.Course
                                  && (p.PurchaseDetails as CoursePurchaseDetails)!.CourseId == course.Id);
        if (maybeAlreadyBought is not null) return Results.Forbid();

        return dto.Price != course.Price ? Results.BadRequest() : null; // check price service
    }

    private async Task<IResult?> ValidateCertificatePurchase(PurchaseRequestDto dto)
    {
        if (dto.CourseId is null) return Results.BadRequest();

        var user = await userRepository.GetAsync(dto.UserId!.Value, true);
        var course = await courseRepository.GetAsync(dto.CourseId!.Value);
        if (user is null || course is null) return Results.NotFound(); // validation service
        if (!course.HasCertificates) return Results.Forbid();

        var maybeAlreadyBought = user.Purchases
            .SingleOrDefault(p => p.PurchaseDetails.PurchaseType is PurchaseType.Certificate
                                  && (p.PurchaseDetails as CertificatePurchaseDetails)!.CourseId == course.Id);
        if (maybeAlreadyBought is not null) return Results.Forbid();

        // курс должен быть бесплатный или уже купленный 
        if (course.Price > 0
            && user.Purchases.SingleOrDefault(p =>
                p.PurchaseDetails.PurchaseType is PurchaseType.Course
                && (p.PurchaseDetails as CoursePurchaseDetails)!.CourseId == course.Id) is null)
            return Results.Forbid();

        var uci = await userCourseInfoRepository.GetAsync(user.Id, course.Id);
        if (uci is null || !uci.IsCompleted) return Results.Forbid();

        return dto.Price != 1000 ? Results.BadRequest() : null; // check price service
    }

    private async Task<IResult?> ValidateSubscriptionPurchase(PurchaseRequestDto dto)
    {
        if (dto.SubscriptionId is null || dto.UserId is null)
            return Results.BadRequest();

        var user = await userRepository.GetAsync(dto.UserId.Value, true, true);
        var subscription = await subscriptionRepository.GetAsync(dto.SubscriptionId.Value);
        if (user is null || subscription is null) return Results.NotFound();


        if (dto.SubscriptionId == (int)SubscriptionType.Free)
            return Results.Forbid();

        var hasActive = user.Purchases
            .SingleOrDefault(
                p => p.PurchaseDetails.PurchaseType is PurchaseType.Subscription
                     && (p.PurchaseDetails as SubscriptionPurchaseDetails)!.SubscriptionId == user.SubscriptionId);
        if (hasActive is not null) return Results.Forbid();

        return dto.Price != subscription.Price ? Results.BadRequest() : null;
    }
}

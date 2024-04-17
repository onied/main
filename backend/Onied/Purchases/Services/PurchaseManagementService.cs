using Purchases.Data.Abstractions;
using Purchases.Data.Enums;
using Purchases.Data.Models;
using Purchases.Data.Models.PurchaseDetails;
using Purchases.Dtos.Requests;
using Purchases.Services.Abstractions;

namespace Purchases.Services;

public class PurchaseManagementService(
    IUserRepository userRepository,
    ICourseRepository courseRepository) : IPurchaseManagementService
{
    public async Task<IResult?> ValidatePurchase(PurchaseRequestDto dto, PurchaseType purchaseType)
    {
        return dto.PurchaseType switch
        {
            PurchaseType.Course => await ValidateCoursePurchase(dto),
            PurchaseType.Certificate => await ValidateCertificatePurchase(dto),
            PurchaseType.Subscription => null,
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

        return dto.Price != 1000 ? Results.BadRequest() : null; // check price service
    }
}

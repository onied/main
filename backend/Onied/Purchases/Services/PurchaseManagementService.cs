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
        if (dto.PurchaseType is not PurchaseType.Course
            || dto.CourseId is null) return Results.BadRequest();

        var user = await userRepository.GetAsync(dto.UserId!.Value, true);
        var course = await courseRepository.GetAsync(dto.CourseId!.Value);
        if (user is null || course is null) return Results.NotFound(); // validation service


        return dto.PurchaseType switch
        {
            PurchaseType.Course => ValidateCoursePurchase(dto, user, course),
            PurchaseType.Certificate => ValidateCertificatePurchase(dto, user, course),
            PurchaseType.Subscription => null,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private IResult? ValidateCoursePurchase(PurchaseRequestDto dto, User user, Course course)
    {
        var maybeAlreadyBought = user.Purchases
            .SingleOrDefault(p => p.PurchaseDetails.PurchaseType is PurchaseType.Course
                                  && (p.PurchaseDetails as CoursePurchaseDetails)!.CourseId == course.Id);
        if (maybeAlreadyBought is not null) return Results.Forbid();

        return dto.Price != course.Price ? Results.BadRequest() : null; // check price service

    }

    private IResult? ValidateCertificatePurchase(PurchaseRequestDto dto, User user, Course course)
    {
        var maybeAlreadyBought = user.Purchases
            .SingleOrDefault(p => p.PurchaseDetails.PurchaseType is PurchaseType.Certificate
                                  && (p.PurchaseDetails as CertificatePurchaseDetails)!.CourseId == course.Id);
        if (maybeAlreadyBought is not null) return Results.Forbid();

        return dto.Price != 1000 ? Results.BadRequest() : null; // check price service
    }
}

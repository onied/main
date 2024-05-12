using AutoMapper;
using Purchases.Data.Abstractions;
using Purchases.Data.Enums;
using Purchases.Data.Models;
using Purchases.Data.Models.PurchaseDetails;
using Purchases.Dtos.Requests;
using Purchases.Dtos.Responses;
using Purchases.Services.Abstractions;

namespace Purchases.Services;

public class PurchaseMakingService(
    IMapper mapper,
    IValidatePurchaseService validatePurchaseService,
    IUserRepository userRepository,
    ICourseRepository courseRepository,
    ISubscriptionRepository subscriptionRepository,
    IPurchaseRepository purchaseRepository,
    IPurchaseTokenService tokenService,
    IPurchaseCreatedProducer purchaseCreatedProducer,
    ISubscriptionChangedProducer subscriptionChangedProducer
) : IPurchaseMakingService
{
    public async Task<IResult> GetCoursePreparedPurchase(int courseId)
    {
        var course = await courseRepository.GetAsync(courseId);
        if (course is null) return Results.NotFound();

        var coursePurchaseInfo = mapper.Map<PreparedPurchaseResponseDto>(course) with
        {
            PurchaseType = PurchaseType.Course
        };
        return Results.Ok(coursePurchaseInfo);
    }

    public async Task<IResult> MakeCoursePurchase(PurchaseRequestDto dto, Guid userId)
    {
        dto = dto with { UserId = userId };
        var maybeError = await validatePurchaseService.ValidatePurchase(dto, PurchaseType.Course);
        if (maybeError is not null) return maybeError;

        var purchase = mapper.Map<Purchase>(dto);

        var purchaseDetails = new CoursePurchaseDetails()
        {
            PurchaseType = PurchaseType.Course,
            CourseId = dto.CourseId!.Value,
            PurchaseDate = DateTime.UtcNow
        };

        purchase = await purchaseRepository.AddAsync(purchase, purchaseDetails);
        purchase.Token = tokenService.GetToken(purchase);
        await purchaseRepository.UpdateAsync(purchase);
        await purchaseCreatedProducer.PublishAsync(purchase);
        return Results.Ok();
    }

    public async Task<IResult> GetCertificatePreparedPurchase(int courseId)
    {
        var course = await courseRepository.GetAsync(courseId);
        if (course is null) return Results.NotFound();

        var coursePurchaseInfo = new PreparedPurchaseResponseDto(course.Title, 1000, PurchaseType.Certificate);
        return Results.Ok(coursePurchaseInfo);
    }

    public async Task<IResult> MakeCertificatePurchase(PurchaseRequestDto dto, Guid userId)
    {
        dto = dto with { UserId = userId };
        var maybeError = await validatePurchaseService.ValidatePurchase(dto, PurchaseType.Certificate);
        if (maybeError is not null) return maybeError;

        var purchase = mapper.Map<Purchase>(dto);

        var purchaseDetails = new CertificatePurchaseDetails
        {
            PurchaseType = PurchaseType.Certificate,
            CourseId = dto.CourseId!.Value,
            PurchaseDate = DateTime.UtcNow
        };

        purchase = await purchaseRepository.AddAsync(purchase, purchaseDetails);
        purchase.Token = tokenService.GetToken(purchase);
        await purchaseRepository.UpdateAsync(purchase);
        await purchaseCreatedProducer.PublishAsync(purchase);
        return Results.Ok();
    }

    public async Task<IResult> GetSubscriptionPreparedPurchase(int subscriptionId)
    {
        var subscription = await subscriptionRepository.GetAsync(subscriptionId);
        if (subscription is null) return Results.NotFound();
        if (subscription.Id == (int)SubscriptionType.Free) return Results.Forbid();

        var purchaseInfo = new PreparedPurchaseResponseDto(
            subscription.Title, subscription.Price, PurchaseType.Subscription);
        return Results.Ok(purchaseInfo);
    }

    public async Task<IResult> MakeSubscriptionPurchase(PurchaseRequestDto dto, Guid userId)
    {
        dto = dto with { UserId = userId };
        var maybeError = await validatePurchaseService.ValidatePurchase(dto, PurchaseType.Subscription);
        if (maybeError is not null) return maybeError;

        var purchase = mapper.Map<Purchase>(dto);

        var purchaseDetails = new SubscriptionPurchaseDetails
        {
            PurchaseType = PurchaseType.Subscription,
            SubscriptionId = dto.SubscriptionId!.Value,
            StartDate = DateTime.Now.ToUniversalTime(),
            EndDate = DateTime.Now.AddMonths(1).ToUniversalTime(),
            AutoRenewalEnabled = true
        };

        purchase = await purchaseRepository.AddAsync(purchase, purchaseDetails);
        purchase.Token = tokenService.GetToken(purchase);
        await purchaseRepository.UpdateAsync(purchase);
        await purchaseCreatedProducer.PublishAsync(purchase);

        var user = (await userRepository.GetAsync(userId))!;
        user.SubscriptionId = dto.SubscriptionId!.Value;
        await userRepository.UpdateAsync(user);

        var subscription = (await subscriptionRepository
            .GetAsync(dto.SubscriptionId.Value))!;
        await subscriptionChangedProducer.PublishAsync(subscription, userId);
        return Results.Ok();
    }
}

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Purchases.Data.Abstractions;
using Purchases.Data.Enums;
using Purchases.Data.Models;
using Purchases.Data.Models.PurchaseDetails;
using Purchases.Dtos.Requests;
using Purchases.Dtos.Responses;
using Purchases.Services.Abstractions;

namespace Purchases.Controllers;

[Route("api/v1/purchases/new")]
public class PurchasesMakingController(
    IMapper mapper,
    IPurchaseManagementService purchaseManagementService,
    ICourseRepository courseRepository,
    IPurchaseRepository purchaseRepository,
    IPurchaseTokenService tokenService,
    IPurchaseCreatedProducer purchaseCreatedProducer) : ControllerBase
{
    [HttpGet("course")]
    public async Task<IResult> GetCoursePreparedPurchase([FromQuery] int courseId)
    {
        var course = await courseRepository.GetAsync(courseId);
        if (course is null) return Results.NotFound();

        var coursePurchaseInfo = mapper.Map<PreparedPurchaseResponseDto>(course) with
        {
            PurchaseType = PurchaseType.Course
        };
        return Results.Ok(coursePurchaseInfo);
    }

    [HttpPost("course")]
    public async Task<IResult> MakeCoursePurchase([FromBody] PurchaseRequestDto dto, [FromQuery] Guid userId)
    {
        dto = dto with { UserId = userId };
        var maybeError = await purchaseManagementService.ValidatePurchase(dto, PurchaseType.Course);
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

    [HttpGet("certificate")]
    public async Task<IResult> GetCertificatePreparedPurchase([FromQuery] int courseId)
    {
        var course = await courseRepository.GetAsync(courseId);
        if (course is null) return Results.NotFound();

        var coursePurchaseInfo = new PreparedPurchaseResponseDto(course.Title, 1000, PurchaseType.Certificate);
        return Results.Ok(coursePurchaseInfo);
    }

    [HttpPost("certificate")]
    public async Task<IResult> MakeCertificatePurchase([FromBody] PurchaseRequestDto dto, [FromQuery] Guid userId)
    {
        dto = dto with { UserId = userId };
        var maybeError = await purchaseManagementService.ValidatePurchase(dto, PurchaseType.Certificate);
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
}

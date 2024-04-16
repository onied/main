using AutoMapper;
using MassTransit.Data.Messages;
using Microsoft.AspNetCore.Mvc;
using Purchases.Abstractions;
using Purchases.Data.Abstractions;
using Purchases.Data.Enums;
using Purchases.Data.Models;
using Purchases.Data.Models.PurchaseDetails;
using Purchases.Dtos.Requests;
using Purchases.Dtos.Responses;
using Purchases.Producers.PurchaseCreatedProducer;

namespace Purchases.Controllers;

[Route("api/v1/purchases/new")]
public class PurchasesMakingController(
    IMapper mapper,
    IUserRepository userRepository,
    ICourseRepository courseRepository,
    IPurchaseRepository purchaseRepository,
    IPurchaseTokenService tokenService,
    IPurchaseCreatedProducer purchaseCreatedProducer) : ControllerBase
{
    [HttpGet("course/{courseId:int}")]
    public async Task<IResult> GetCoursePreparedPurchase(int courseId)
    {
        var course = await courseRepository.GetAsync(courseId);
        if (course is null) return TypedResults.NotFound();

        var coursePurchaseInfo = mapper.Map<PreparedPurchaseResponseDto>(course) with
        {
            PurchaseType = PurchaseType.Course
        };
        return TypedResults.Ok(coursePurchaseInfo);
    }

    [HttpPost("course/{courseId:int}")]
    public async Task<IResult> MakeCoursePurchase(int courseId, [FromBody] PurchaseRequestDto dto, Guid userId)
    {
        dto = dto with { UserId = userId };
        if (dto.PurchaseType is not PurchaseType.Course
            || dto.CourseId is null) return TypedResults.BadRequest();

        var user = await userRepository.GetAsync(dto.UserId!.Value);
        var course = await courseRepository.GetAsync(dto.CourseId!.Value);
        if (user is null || course is null) return TypedResults.NotFound(); // validation service

        var purchase = mapper.Map<Purchase>(dto);

        if (purchase.Price != course.Price) return TypedResults.BadRequest(); // price check service
        var purchaseDetails = new CoursePurchaseDetails()
        {
            PurchaseType = PurchaseType.Course,
            CourseId = dto.CourseId!.Value,
        };
        purchase = await purchaseRepository.AddAsync(purchase, purchaseDetails);
        purchase.Token = tokenService.GetToken(purchase);
        await purchaseRepository.UpdateAsync(purchase);

        await purchaseCreatedProducer.PublishAsync(purchase);
        return TypedResults.Ok();
    }
}

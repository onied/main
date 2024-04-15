using System.Diagnostics;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Purchases.Data.Abstractions;
using Purchases.Data.Models;
using Purchases.Data.Models.PurchaseDetails;
using Purchases.Dtos;

namespace Purchases.Controllers;

public class PurchasesController(
    IMapper mapper,
    IUserRepository userRepository,
    ICourseRepository courseRepository,
    IPurchaseRepository purchaseRepository) : ControllerBase
{
    [HttpGet]
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

    [HttpPost]
    public async Task<IResult> MakeCoursePurchase([FromBody] PurchaseRequestDto dto, Guid userId)
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
        await purchaseRepository.AddAsync(purchase, purchaseDetails);
        return TypedResults.Ok();
    }
}

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Purchases.Data.Abstractions;
using Purchases.Data.Models;
using Purchases.Dtos;

namespace Purchases.Controllers;

public class PurchasesController(
    IMapper mapper,
    ICourseRepository courseRepository) : ControllerBase
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
}

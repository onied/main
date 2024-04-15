using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Purchases.Data.Abstractions;
using Purchases.Data.Models;
g

namespace Purchases.Controllers;

[Route("api/v1/[controller]")]
public class PurchasesController(
    IMapper mapper,
    IUserRepository userRepository) : ControllerBase
{
    [HttpGet]
    public async Task<IResult> GetPurchasesByUser(Guid userId)
    {
        var user = await userRepository.GetAsync(userId, true);
        if (user is null) return TypedResults.NotFound();

        var pinfo = mapper.Map<List<Purchase>>(user.Purchases);
        return TypedResults.Ok(pinfo);
    }
}

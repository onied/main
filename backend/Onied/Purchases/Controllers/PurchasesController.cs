using Microsoft.AspNetCore.Mvc;
using Purchases.Data.Abstractions;

namespace Purchases.Controllers;

[Route("api/v1/[controller]")]
public class PurchasesController(IUserRepository userRepository) : ControllerBase
{
    [HttpGet]
    public async Task<IResult> GetPurchasesByUser(Guid userId)
    {
        var user = await userRepository.GetAsync(userId, true);
        if (user is null) return TypedResults.NotFound();

        return TypedResults.Ok(user.Purchases);
    }
}

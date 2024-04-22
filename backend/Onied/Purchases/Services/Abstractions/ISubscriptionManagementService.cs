using Purchases.Data.Models;

namespace Purchases.Services.Abstractions;

public interface ISubscriptionManagementService
{
    public Task<IResult> GetSubscriptionsByUser(Guid userId);
}

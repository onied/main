using Grpc.Core;
using Purchases.Data.Abstractions;
using Purchases.Services.Abstractions;
using PurchasesGrpc;

namespace Purchases.Services.GrpcServices;

public class PurchasesService(
    IUserRepository userRepository,
    IPurchaseRepository purchaseRepository,
    IPurchaseTokenService tokenService) : PurchasesGrpc.PurchasesService.PurchasesServiceBase
{
    public override async Task<VerifyTokenReply> Verify(VerifyTokenRequest request, ServerCallContext context)
    {
        var purchaseTokenInfo = tokenService.ConvertToPurchaseTokenInfo(request.Token);

        var user = await userRepository.GetAsync(purchaseTokenInfo.UserId, true);
        var purchase = await purchaseRepository.GetAsync(purchaseTokenInfo.Id);
        if (user is null || purchase is null)
            return new VerifyTokenReply { VerificationOutcome = VerificationOutcome.BadRequest };

        return purchase.Token!.Equals(request.Token)
            ? new VerifyTokenReply { VerificationOutcome = VerificationOutcome.Ok }
            : new VerifyTokenReply { VerificationOutcome = VerificationOutcome.Forbid };
    }
}

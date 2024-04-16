using JWT.Algorithms;
using JWT.Builder;
using Purchases.Abstractions;

namespace Purchases.Services;

public class JwtTokenService(string secretKey) : IJwtTokenService
{
    public string GenerateToken(Dictionary<string, object?> claims)
        => claims.Aggregate(
                JwtBuilder.Create()
                    .WithAlgorithm(new HMACSHA256Algorithm())
                    .WithSecret(secretKey),
                (b, pair) => b.AddClaim(pair.Key, pair.Value))
            .Encode();
}

namespace Purchases.Abstractions;

public interface IJwtTokenService
{
    public string GenerateToken(Dictionary<string, object?> claims);
}

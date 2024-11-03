namespace Purchases.Services.Abstractions;

public interface IJwtTokenService
{
    public string GenerateToken(Dictionary<string, object?> claims);
    public string DecodeToken(Dictionary<string, object?> claims, string token);
}

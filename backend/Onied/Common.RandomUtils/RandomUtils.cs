namespace Common.RandomUtils;

public static class Utils
{
    public static string GetRandomString(int length)
    {
        var bytes = new byte[length];
        Random.Shared.NextBytes(bytes);
        return Convert.ToBase64String(bytes);
    }
}

namespace Users.Dtos.VkOauth.Request;

public class OauthCodeRequest
{
    /// <summary>
    ///     Временный код, полученный после прохождения авторизации.
    /// </summary>
    public string Code { get; set; } = null!;

    /// <summary>
    ///     URL, который использовался при получении code на первом этапе авторизации. Должен быть аналогичен переданному при
    ///     авторизации.
    /// </summary>
    public string RedirectUri { get; set; } = null!;
}

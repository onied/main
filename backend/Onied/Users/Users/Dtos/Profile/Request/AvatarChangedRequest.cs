using System.ComponentModel.DataAnnotations;

namespace Users.Dtos.Profile.Request;

public class AvatarChangedRequest
{
    [Url]
    [MaxLength(2048)]
    public string? AvatarHref { get; set; }
}

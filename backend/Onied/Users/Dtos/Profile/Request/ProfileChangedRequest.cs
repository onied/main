using System.ComponentModel.DataAnnotations;

namespace Users.Dtos.Profile.Request;

public class ProfileChangedRequest
{
    [MaxLength(50)]
    public string FirstName { get; set; } = null!;

    [MaxLength(50)]
    public string LastName { get; set; } = null!;

    [Range(0, 2)]
    public Gender Gender { get; set; }
}

using System.ComponentModel.DataAnnotations;
using Users.Data.Enums;

namespace Users.Dtos.Users.Request;

public class RegisterUserRequest
{
    [MaxLength(50)]
    public string FirstName { get; set; } = null!;

    [MaxLength(50)]
    public string LastName { get; set; } = null!;

    public Gender Gender { get; set; }

    [EmailAddress]
    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;
}

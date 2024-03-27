using System.ComponentModel.DataAnnotations;

namespace Users.Dtos;

public class RegisterUserDto
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

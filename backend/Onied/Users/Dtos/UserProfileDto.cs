using System.ComponentModel.DataAnnotations;

namespace Users.Dtos;

public class UserProfileDto
{
    [MaxLength(50)]
    public string? FirstName { get; set; } = null!;

    [MaxLength(50)]
    public string? LastName { get; set; } = null!;

    public Gender? Gender { get; set; }

    [Url]
    [MaxLength(2048)]
    public string? Avatar { get; set; } = null!;

    [EmailAddress]
    public string? Email { get; set; } = null!;
}

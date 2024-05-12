using System.ComponentModel.DataAnnotations;
using Users.Data;

namespace Users.Dtos;

public class UserProfileDto
{
    public string? FirstName { get; set; } = null!;

    public string? LastName { get; set; } = null!;

    public Gender? Gender { get; set; }

    public string? Avatar { get; set; } = null!;

    public string? Email { get; set; } = null!;
}

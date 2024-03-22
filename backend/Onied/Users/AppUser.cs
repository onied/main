using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Users;

public class AppUser : IdentityUser
{
    [MaxLength(50)]
    public string? FirstName { get; set; } = null!;

    [MaxLength(50)]
    public string? LastName { get; set; } = null!;

    public Gender? Gender { get; set; }

    [Url]
    [MaxLength(2048)]
    public string? Avatar { get; set; }
}

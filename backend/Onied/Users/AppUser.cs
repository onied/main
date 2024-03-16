using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Users;

public class AppUser : IdentityUser
{
    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; } = null!;

    [Required]
    [MaxLength(50)]
    public string LastName { get; set; } = null!;

    public bool? Gender { get; set; }
}

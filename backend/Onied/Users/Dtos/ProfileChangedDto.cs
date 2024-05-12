using System.ComponentModel.DataAnnotations;
using Users.Data;

namespace Users.Dtos;

public class ProfileChangedDto
{
    [MaxLength(50)]
    public string FirstName { get; set; } = null!;

    [MaxLength(50)]
    public string LastName { get; set; } = null!;

    [Range(0, 2)]
    public Gender Gender { get; set; }
}

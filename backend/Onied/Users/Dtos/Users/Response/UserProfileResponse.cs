namespace Users.Dtos.Users.Response;
using Users.Data;
public class UserProfileResponse
{
    public string? FirstName { get; set; } = null!;

    public string? LastName { get; set; } = null!;

    public Gender? Gender { get; set; }

    public string? Avatar { get; set; } = null!;

    public string? Email { get; set; } = null!;
}

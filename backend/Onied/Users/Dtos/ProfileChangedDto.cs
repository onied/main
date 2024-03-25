namespace Users.Dtos;

public class ProfileChangedDto
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public Gender Gender { get; set; }
}

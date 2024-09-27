using MassTransit.Data.Enums;

namespace MassTransit.Data.Messages;

public class ProfileUpdated
{
    public Guid Id { get; init; }
    public string FirstName { get; init; } = null!;
    public string LastName { get; init; } = null!;
    public Gender? Gender { get; init; }
}

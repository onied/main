using MassTransit.Data.Enums;

namespace MassTransit.Data.Messages;

public record UserCreated
{
    public Guid Id { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public Gender? Gender { get; init; }
    public string? AvatarHref { get; init; }
}

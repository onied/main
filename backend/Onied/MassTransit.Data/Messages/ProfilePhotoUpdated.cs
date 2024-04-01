namespace MassTransit.Data.Messages;

public class ProfilePhotoUpdated
{
    public Guid Id { get; init; }
    public string? AvatarHref { get; init; }
}

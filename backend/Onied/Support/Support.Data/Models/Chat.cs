namespace Support.Data.Models;

public class Chat
{
    /// <summary>
    ///     Chat public ID. Used by support users. Should not be shown to regular users.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    ///     User id from Users service. There should be no more than one chat per user.
    /// </summary>
    public Guid ClientId { get; set; }

    /// <summary>
    ///     Support user who is attending to the current session in this chat.
    /// </summary>
    public SupportUser? Support { get; set; }
    public Guid? SupportId { get; set; }

    public Guid? CurrentSessionId { get; set; }

    public ICollection<MessageView> Messages { get; } = new List<MessageView>();
}

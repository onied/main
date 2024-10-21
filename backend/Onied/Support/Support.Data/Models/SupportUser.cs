namespace Support.Data.Models;

public class SupportUser
{
    /// <summary>
    ///     This should be a regular user id from Users service.
    ///     We don't need to store all info on users from Users service,
    ///     just all the relevant info for support users.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    ///     Support user number. Instead of identifying support users by their almost-unreadable GUID,
    ///     or de-anonymizing them by flashing their name,
    ///     this property allows for human-readable regular number identifier.
    /// </summary>
    public int Number { get; set; }

    public ICollection<Chat> ActiveChats { get; } = new List<Chat>();
}

namespace Support.Helpers;

public class ChatHubContextItemsHelper(IDictionary<object, object?> dictionary)
{
    private const string IsSupportUserItem = "IsSupportUser";
    private const string UserIdItem = "UserId";

    public bool IsSupportUser
    {
        get => (bool)(dictionary[IsSupportUserItem] ?? false);
        set => dictionary[IsSupportUserItem] = value;
    }

    public Guid UserId
    {
        get => (Guid)(dictionary[UserIdItem] ?? Guid.Empty);
        set => dictionary[UserIdItem] = value;
    }
}

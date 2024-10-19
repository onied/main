namespace Courses.Attributes;

public class StringValueAttribute(string value) : Attribute
{
    public string StringValue { get; protected set; } = value;
}

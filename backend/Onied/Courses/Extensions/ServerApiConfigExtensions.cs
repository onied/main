using Courses.Attributes;

namespace Courses.Extensions;

public static class ServerApiConfigExtensions
{
    public static string? GetStringValue(this Enum value)
    {
        var type = value.GetType();

        var fieldInfo = type.GetField(value.ToString())!;

        var attribs = fieldInfo.GetCustomAttributes(
            typeof(StringValueAttribute), false) as StringValueAttribute[];

        return attribs?.Length > 0 ? attribs[0].StringValue : null;
    }
}

using System.Text.Json.Serialization;
using System.Text.Json;

namespace Ecom.Services;

public static class StringExtensions
{
    private static readonly JsonSerializerOptions _options = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        ReferenceHandler = ReferenceHandler.IgnoreCycles,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public static string ToJson<T>(this T item) => item == null ? string.Empty : JsonSerializer.Serialize(item, _options);

    public static T? FromJson<T>(this string value) => JsonSerializer.Deserialize<T>(value, _options);
}

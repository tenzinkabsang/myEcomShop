using Newtonsoft.Json;

namespace Ecom.Web.Infrastructure;

public static class Util
{
    private static readonly JsonSerializerSettings SerializerSettings = new() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };

    public static string ToJson(this object value, bool format = true)
    {
        var formatting = format ? Formatting.Indented : Formatting.None;
        return JsonConvert.SerializeObject(value, formatting, SerializerSettings);
    }

    public static T? FromJson<T>(this string? json)
    {
        if (json == null)
            return default;

        return JsonConvert.DeserializeObject<T>(json);
    }

    public static bool IsExpired(this DateTime value) => DateTime.Now.CompareTo(value) > 0;
}

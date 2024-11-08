using Ecom.Core.Extensions;

namespace Ecom.Web.Infrastructure;

public static class SessionExtensions
{
    public static void SetJson(this ISession session, string key, object value)
    {
        session.SetString(key, value.ToJson());
    }

    public static T? GetJson<T>(this ISession session, string key)
    {
        var sessionData = session.GetString(key);
        return string.IsNullOrEmpty(sessionData)
            ? default
            : sessionData.FromJson<T>();
    }
}

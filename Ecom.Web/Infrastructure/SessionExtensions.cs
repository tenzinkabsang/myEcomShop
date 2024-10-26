namespace Ecom.Web.Infrastructure;

public static class SessionExtensions
{
    public static void SetJson(this ISession session, string key, object value)
    {
        session.SetString(key, value.ToJson(format: false));
    }

    public static T? GetJson<T>(this ISession session, string key)
    {
        string? sessionData = session.GetString(key);
        return sessionData.FromJson<T>();
    }
}

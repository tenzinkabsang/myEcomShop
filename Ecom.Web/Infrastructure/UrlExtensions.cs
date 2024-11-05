namespace Ecom.Web.Infrastructure;

public static class UrlExtensions
{
    public static string PathAndQuery(this HttpRequest request) =>
        request.QueryString.HasValue
            ? $"{request.Path}{request.QueryString}"
            : request.Path.ToString();


    public static string ImageUrl(this HttpRequest request, string imagePath)
    {
        if (request == null || string.IsNullOrWhiteSpace(imagePath))
            return string.Empty;

        var uriBuilder = new UriBuilder(request.Scheme, request.Host.Host, request.Host.Port ?? -1);

        // -1 represents no port. Don't show the port number if it's the default for the scheme
        if (uriBuilder.Uri.IsDefaultPort)
            uriBuilder.Port = -1;

        return $"{uriBuilder.Uri.AbsoluteUri}{imagePath}";
    }
}


using System.Security.Cryptography;
using System.Text;

namespace Ecom.Core;

public static class HashHelper
{
    public static string CreateHash(string rawData)
    {
        byte[] bytes = SHA1.HashData(Encoding.UTF8.GetBytes(rawData));

        var builder = new StringBuilder();
        foreach (var b in bytes)
        {
            builder.Append(b.ToString("x2"));
        }

        return builder.ToString();
    }
}

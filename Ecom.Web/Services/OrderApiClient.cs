using Ecom.Core.Domain;
using Ecom.Web.Services.Interfaces;

namespace Ecom.Web.Services;

public class OrderApiClient(HttpClient httpClient) : IOrderApiClient
{
    public int ProcessCheckout(Order order)
    {
        return 1;
    }
}
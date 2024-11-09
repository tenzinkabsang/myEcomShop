using AutoMapper;
using Ecom.Core.Domain;
using Ecom.Core.Extensions;
using Ecom.Web.Models;
using Ecom.Web.Services.Interfaces;

namespace Ecom.Web.Services;

public class OrderApiClient(HttpClient httpClient, IMapper mapper) : IOrderApiClient
{
    public async Task<int?> ProcessCheckout(OrderViewModel model)
    {
        var value = mapper.Map<Order>(model);
        var httpResponse = await httpClient.PostAsJsonAsync("checkout", value);
        var content = await httpResponse.Content.ReadAsStringAsync();
        var order = content.FromJson<CreateOrderResponse>();
        return order?.OrderId;
    }

    private record CreateOrderResponse(int OrderId, int CustomerId);
}
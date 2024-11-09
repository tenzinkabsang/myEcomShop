using Ecom.Core.Domain;
using Ecom.Web.Models;

namespace Ecom.Web.Services.Interfaces;

public interface IOrderApiClient
{
    /// <summary>
    /// Process the given order.
    /// </summary>
    /// <returns>OrderId</returns>
    Task<int?> ProcessCheckout(OrderViewModel model);
}

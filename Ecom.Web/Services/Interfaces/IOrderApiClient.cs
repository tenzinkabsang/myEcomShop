using Ecom.Core.Domain;

namespace Ecom.Web.Services.Interfaces;

public interface IOrderApiClient
{
    int ProcessCheckout(Order order);
}

using AutoMapper;
using Ecom.Core.Domain;
using Ecom.Orders.Api.Endpoints.ShoppingCart;

namespace Ecom.Orders.Api.Mapper;

public class EntityToViewModelMapper : Profile
{
    public EntityToViewModelMapper()
    {
        CreateMap<ShoppingCartItem, ShoppingCartItemDto>();
    }
}

using AutoMapper;
using Ecom.Core.Domain;
using Ecom.Web.Models;

namespace Ecom.Web.Infrastructure.Mapper;

public class ViewModelToEntityMapper : Profile
{
    public ViewModelToEntityMapper()
    {
        CreateMap<ProductViewModel, Product>();

        CreateMap<LineItem, OrderItem>()
            .ForMember(dest => dest.Quantity, options => options.MapFrom(src => src.Quantity))
            .ForMember(dest => dest.ProductId, options => options.MapFrom(src => src.Product.Id));


        CreateMap<OrderViewModel, Order>()
            .ForMember(dest => dest.ShippingAddress, options => options.MapFrom(
                src => new Address
                {
                    Line1 = src.Line1,
                    City = src.City,
                    State = src.State,
                    Zip = src.Zip
                }))
            .ForMember(dest => dest.OrderItems, options => options.MapFrom(src => src.Items))
            .ForMember(dest => dest.CreatedDateUtc, options => options.Ignore())
            .ForMember(dest => dest.ModifiedDateUtc, options => options.Ignore());

    }
}
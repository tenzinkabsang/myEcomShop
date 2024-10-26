using AutoMapper;
using Ecom.Core.Domain;
using Ecom.Web.Models;

namespace Ecom.Web.Infrastructure.Mapper;

public class ViewModelToEntityMapper : Profile
{
    public ViewModelToEntityMapper()
    {
        CreateMap<ProductViewModel, Product>();
    }
}
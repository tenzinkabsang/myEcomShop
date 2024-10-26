﻿using AutoMapper;
using Ecom.Core.Domain;
using Ecom.Web.Models;

namespace Ecom.Web.Infrastructure.Mapper;

public class EntityToViewModelMapper : Profile
{
    public EntityToViewModelMapper()
    {
        CreateMap<Product, ProductViewModel>();
    }
}
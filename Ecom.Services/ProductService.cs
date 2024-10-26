﻿using Ecom.Core;
using Ecom.Core.Domain;
using Ecom.Data;
using Microsoft.EntityFrameworkCore;

namespace Ecom.Services;

public class ProductService(IRepository<Product> productRepository) : IProductService
{
    public async Task<IPagedList<Product>> GetProducts(string? category, int page, int pageSize)
    {
        return await productRepository.GetAllPagedAsync(
            query => query.Include(x => x.Images).Where(p => category == null || p.Category == category).OrderBy(p => p.Id),
            pageIndex: page,
            pageSize: pageSize,
            includeDeleted: false);
    }

    public async Task<Product> GetProduct(int id)
    {
        var product = await productRepository.GetByIdAsync(id, includeDeleted: false);

        if (product is null)
            throw new ArgumentNullException($"No product found for id: {id}");

        return product;
    }

    public async Task<List<string>> GetAllCategoriesAsync()
    {
        return (await productRepository.GetAllAsync(query => query.DistinctBy(p => p.Category)))
            .Select(p => p.Category)
            .OrderBy(x => x)
            .ToList();
    }
}

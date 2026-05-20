using System;
using Core.Entities;

namespace Core.Interfaces;

public interface IProductRepository : IGenericRepository<Product>
{
    Task<IReadOnlyList<string>> GetProductTypesAsync();
}

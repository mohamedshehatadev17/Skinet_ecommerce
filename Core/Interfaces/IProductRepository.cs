using System;
using Core.Entities;

namespace Core.Interfaces;

public interface IProductRepository
{
    Task<IReadOnlyList<Product>> GetProductsAsync(string? brand = null, string? type = null,string? sort = null);
    Task<IReadOnlyList<string>> GetProductTypesAsync();
    Task<Product?> GetProductByIdAsync(int id);
    Task AddAsync(Product product,CancellationToken cancellationToken = default);
    Task UpdateAsync(Product product,CancellationToken cancellationToken = default);
    Task DeleteAsync(Product product,CancellationToken cancellationToken = default);
    Task<bool> ProductExists(int id, CancellationToken cancellationToken = default);
    Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);
}

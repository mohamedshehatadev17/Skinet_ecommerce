using System;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ProductRepository(StoreContext context) : IProductRepository
{
    private readonly StoreContext _context = context;
        public async Task<Product?> GetProductByIdAsync(int id)
    {
        return await _context.Products.FindAsync(id);
    }
    public async Task<IReadOnlyList<Product>> GetProductsAsync(string? brand = null, string? type = null,string? sort = null)
    {           
        var query = _context.Products.AsQueryable();
        if (!string.IsNullOrEmpty(brand))
            query = query.Where(p => p.Brand == brand);
        if (!string.IsNullOrEmpty(type))
            query = query.Where(p => p.Type == type);

        query = sort!.ToLower() switch
        {
            "desc" => query.OrderBy(p => p.Price),
            "asc" => query.OrderBy(p => p.Name),
            _ => query.OrderBy(p => p.Name)
        };
        return await query.ToListAsync();
    }
    public async Task<IReadOnlyList<string>> GetProductBrandsAsync()
    {
        return await _context.Products.Select(x => x.Brand).Distinct().ToListAsync();
    }
    public async Task<IReadOnlyList<string>> GetProductTypesAsync()
    {
        return await _context.Products.Select(x => x.Type).Distinct().ToListAsync();
    }
    public async Task AddAsync(Product product, CancellationToken cancellationToken = default)
    {
        await _context.Products.AddAsync(product, cancellationToken);
    }
    public async Task DeleteAsync(Product product, CancellationToken cancellationToken = default)
    {
        _context.Products.Remove(product);
    }

    public async Task<bool> ProductExists(int id, CancellationToken cancellationToken = default)
    {
        var product = await _context.Products.FindAsync(id);
        return product != null;
    }
    public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
      return await _context.SaveChangesAsync(cancellationToken)>0;
    }
    public async Task UpdateAsync(Product product, CancellationToken cancellationToken = default)
    {
        _context.Entry(product).State = EntityState.Modified;
    }
}

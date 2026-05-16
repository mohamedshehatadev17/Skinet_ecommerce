using System;
using System.Text.Json;
using Core.Entities;

namespace Infrastructure.Data;

public class SeedDataContext
{
    public static async Task SeedDataAsync(StoreContext context)
    {
        if (!context.Products.Any())
        {
            var products = await File.ReadAllTextAsync("../Infrastructure/Data/SeedData/products.json");
            var productsList = JsonSerializer.Deserialize<List<Product>>(products);
            if (productsList is null) return;
            context.Products.AddRange(productsList!);
            await context.SaveChangesAsync();
        }
    }
}

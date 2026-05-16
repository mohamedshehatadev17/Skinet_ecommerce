using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductRepository _repository;

    public ProductsController(IProductRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts(string? brand = null, string? type = null,string? sort = null)
    {
        return Ok(await _repository.GetProductsAsync(brand, type, sort));
    }
    [HttpGet("brands")]
  
    [HttpGet("{id:int}")] // api/products/2
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await _repository.GetProductByIdAsync(id);
        return product is null ? NotFound() : Ok(product);
    }   

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        await _repository.AddAsync(product);
        return await _repository.SaveChangesAsync()
                                    ? CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product)
                                    : BadRequest("can not create the product");
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateProduct(int id)
    {
        var existingProduct = await _repository.GetProductByIdAsync(id);
        if (existingProduct is null) return NotFound();
        await _repository.UpdateAsync(existingProduct);        
        return await _repository.SaveChangesAsync() 
                                    ? NoContent() 
                                    : BadRequest("can not update the product");
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        var product = await _repository.GetProductByIdAsync(id);
        if (product is null) return NotFound();
        await _repository.DeleteAsync(product);
        return await _repository.SaveChangesAsync() 
                                    ? NoContent() 
                                    : BadRequest("can not delete the product");
    }
}

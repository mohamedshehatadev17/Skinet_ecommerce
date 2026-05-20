using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
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
        var spec = new ProductSpecification(brand, type);
        var products = await _repository.ListAsync(spec);
        return Ok(products);
    }
    [HttpGet("brands")]
  
    [HttpGet("{id:int}")] // api/products/2
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await _repository.GetByIdAsync(id);
        return product is null ? NotFound() : Ok(product);
    }   

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        await _repository.AddAsync(product);
        return await _repository.SaveAllAsync()
                                    ? CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product)
                                    : BadRequest("can not create the product");
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateProduct(int id)
    {
        var existingProduct = await _repository.GetByIdAsync(id);
        if (existingProduct is null) return NotFound();
        await _repository.UpdateAsync(existingProduct);        
        return await _repository.SaveAllAsync() 
                                    ? NoContent() 
                                    : BadRequest("can not update the product");
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        var product = await _repository.GetByIdAsync(id);
        if (product is null) return NotFound();
        await _repository.DeleteAsync(product);
        return await _repository.SaveAllAsync() 
                                    ? NoContent() 
                                    : BadRequest("can not delete the product");
    }
}

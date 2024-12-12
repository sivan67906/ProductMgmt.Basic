using Microsoft.AspNetCore.Mvc;
using ProductMgmt.Application.Services;
using ProductMgmt.Domain.Interfaces;
using ProductMgmt.Domain.Product;

namespace ProductMgmt.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IGenericRepository<Product> _productRepository;
    private readonly IProductService _productService;

    public ProductController(IGenericRepository<Product> productRepository
                            , IProductService productService)
    {
        _productRepository = productRepository;
        _productService = productService;
    }

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll()
    {
        var products = await _productRepository.GetAllAsync();
        if (products is null) return NotFound();
        return Ok(products);
    }

    [HttpGet("GetById")]
    public async Task<IActionResult> GetById(int Id)
    {
        var product = await _productRepository.GetByIdAsync(Id);
        if (product is null) return NotFound();
        return Ok(product);
    }

    [HttpGet("{productName:required}")]
    public async Task<IActionResult> GetByProductName(string productName)
    {
        var product = await _productService.SearchProductsByNameAsync(productName);
        if (product is null) return NotFound();
        return Ok(product);
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchProductsByName(string searchProduct)
    {
        var products = await _productService.SearchProductsByNameAsync(searchProduct);
        if (products is null || !products.Any()) return NotFound("Searchable product not found.");
        return Ok(products);
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create(Product entity)
    {
        if (entity == null) return BadRequest();
        await _productRepository.CreateAsync(entity);
        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity);
    }

    [HttpPut("Update")]
    public async Task<IActionResult> UpdateProduct(Product product)
    {
        await _productRepository.UpdateAsync(product);
        return NoContent();
    }

    [HttpDelete("Delete")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var existingProduct = await _productRepository.GetByIdAsync(id);
        if (existingProduct is null) return NotFound();
        await _productRepository.DeleteAsync(existingProduct);
        return NoContent();
    }



}
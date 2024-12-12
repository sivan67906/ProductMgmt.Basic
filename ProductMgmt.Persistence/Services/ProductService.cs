using ProductMgmt.Application.Services;
using ProductMgmt.Domain.Interfaces;
using ProductMgmt.Domain.Product;

namespace ProductMgmt.Persistence.Services;

public class ProductService : IProductService
{
    private readonly IGenericRepository<Product> _productRepository;

    public ProductService(IGenericRepository<Product> productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Product> GetByProductNameAsync(string productName)
    {
        var product = await _productRepository.GetAllAsync();
        return product.FirstOrDefault(
            p => p.Name.Equals(productName, StringComparison.OrdinalIgnoreCase));
    }

    public async Task<IEnumerable<Product>> SearchProductsByNameAsync(string productName)
    {
        var products = await _productRepository.GetAllAsync();
        return products.Where(
            p => p.Name.Contains(productName, StringComparison.OrdinalIgnoreCase));
    }

    public async Task UpdateProductAsync(Product updateProduct)
    {
        var existingProduct = await _productRepository.GetByIdAsync(updateProduct.Id);

        if (existingProduct == null)
        {
            throw new KeyNotFoundException($"Product with ID {updateProduct.Id} not found.");
        }

        // Detach the existing product to avoid tracking conflict
        _productRepository.Detach(existingProduct);

        // Apply changes to the product
        existingProduct.Name = updateProduct.Name;
        existingProduct.Description = updateProduct.Description;
        existingProduct.Price = updateProduct.Price;
        existingProduct.StockQuantity = updateProduct.StockQuantity;
        existingProduct.Category = updateProduct.Category;
        existingProduct.SKU = updateProduct.SKU;
        existingProduct.ImageUrl = updateProduct.ImageUrl;
        existingProduct.UpdatedDate = updateProduct.UpdatedDate;
        existingProduct.IsActive = updateProduct.IsActive;

        // Call the repository's UpdateAsync method
        await _productRepository.UpdateAsync(existingProduct);
    }
}

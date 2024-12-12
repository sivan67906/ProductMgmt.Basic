using ProductMgmt.Domain.Product;

namespace ProductMgmt.Application.Services;

public interface IProductService
{
    Task<Product> GetByProductNameAsync(string productName);
    Task<IEnumerable<Product>> SearchProductsByNameAsync(string productName);
    Task UpdateProductAsync(Product product);
}

using ProductManagement.Models;
using System.Runtime.CompilerServices;

namespace ProductManagement.Contracts
{
    public interface IProductService 
    {
        Task CreateProduct(Product product); 
        Task UpdateProduct(Product product);
        Task DeleteProduct(Product product);

        Task<Product> GetProductById(int? productId);
        Task<List<Product>> GetProducts();
        bool ProductExists(int productId);
    }
}

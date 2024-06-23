using Microsoft.EntityFrameworkCore;
using ProductManagement.Contracts;
using ProductManagement.Data;
using ProductManagement.Models;
using System.Runtime.CompilerServices;

namespace ProductManagement.Services
{
    public class ProductService : IProductService
    {
        private readonly ProductDbContext _context;

        public ProductService(ProductDbContext context)
        {
            _context = context;
        }

        public async Task DeleteProduct(Product product)
        {
            if (product != null)
            {
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();
        }

        public bool ProductExists(int productId)
        {
            return (_context.Products?.Any(e => e.ProductId == productId)).GetValueOrDefault();
        }

        public async Task UpdateProduct(Product product)
        {
            _context.Products.Attach(product);

            // Mark all properties as modified
            _context.Entry(product).State = EntityState.Modified;

            // Exclude specified fields from being marked as modified
            _context.Entry(product).Property("CreatedBy").IsModified = false;
            _context.Entry(product).Property("CreatedDate").IsModified = false;

            // Save changes to the database
            await _context.SaveChangesAsync();
        }

        public async Task<Product> GetProductById(int? productId)
        {
            var product = await _context.Products
               .Include(p => p.ProductCategory)
               .FirstOrDefaultAsync(m => m.ProductId == productId);

            return product;
        }

        public async Task<List<Product>> GetProducts()
        {
            return await _context.Products.Include(p => p.ProductCategory).ToListAsync();
        }

        public async Task CreateProduct(Product product)
        {
            product.ProductCode = _context.GetNextSequenceValue();
            var p = await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        } 

    } 
}

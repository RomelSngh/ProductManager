using Microsoft.EntityFrameworkCore;
using ProductManagement.Contracts;
using ProductManagement.Data;
using ProductManagement.Models;

namespace ProductManagement.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ProductDbContext _context;

        public CategoryService(ProductDbContext context)
        {
            _context = context;
        }

        public async Task CreateCategory(Category category)
        {
            _context.Add(category);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCategory(Category category)
        {
            if (category != null)
            {
                _context.Categories.Remove(category);
            }

            await _context.SaveChangesAsync();

        }
        public async Task<IEnumerable<Category>> GetAllCategories()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<IEnumerable<Category>> GetCategories()
        {
            return await _context.Categories.Where(c=>c.IsActive==true).ToListAsync();
        }

        public async Task<Category> GetCategoryById(int? id)
        {
            var ct = await _context.Categories
                 .FirstOrDefaultAsync(m => m.CategoryId == id);
            return ct;
        }

        public async Task UpdateCategory(Category category)
        {
            _context.Categories.Attach(category);

            // Mark all properties as modified
            _context.Entry(category).State = EntityState.Modified;

            // Exclude specified fields from being marked as modified
            _context.Entry(category).Property("CreatedBy").IsModified = false;
            _context.Entry(category).Property("CreatedDate").IsModified = false;

            // Save changes to the database
            await _context.SaveChangesAsync();
        }

        public bool CategoryExists(int id)
        {
            return (_context.Categories?.Any(e => e.CategoryId == id)).GetValueOrDefault();
        }
    }
}

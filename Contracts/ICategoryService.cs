using ProductManagement.Models;

namespace ProductManagement.Contracts
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetCategories();
        Task<IEnumerable<Category>> GetAllCategories();

        Task CreateCategory(Category category);
        Task UpdateCategory(Category category);
        Task DeleteCategory(Category category);

        Task<Category> GetCategoryById(int? id);
        bool CategoryExists(int id);
    }
}

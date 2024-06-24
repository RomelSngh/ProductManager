using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProductManagement.Contracts;
using ProductManagement.Data;
using ProductManagement.Models;

namespace ProductManagement.Services
{
    public class CategoryService : ICategoryService
    {
        Uri baseAddress = new Uri("https://localhost:7116/api");
        public readonly HttpClient _http;

        public CategoryService()
        {
            _http = new HttpClient();
            _http.BaseAddress = baseAddress;
        }

        public async Task CreateCategory(Category category)
        {
            string data = JsonConvert.SerializeObject(category);
            StringContent content = new StringContent(data, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _http.PostAsync(baseAddress + "/CategoriesApi", content);
        }

        public async Task DeleteCategory(Category category)
        {
            HttpResponseMessage response = await _http.DeleteAsync(baseAddress + $"/CategoriesApi/{category.CategoryId}");

        }
        public async Task<IEnumerable<Category>> GetAllCategories()
        {
            //return await _context.Categories.ToListAsync();
            List<Category> categories = new List<Category>();
            HttpResponseMessage response =await _http.GetAsync(baseAddress + "/CategoriesApi/GetAllCategories");

            if (!response.IsSuccessStatusCode)
            {
                return new List<Category>();
           }

            string data = response.Content.ReadAsStringAsync().Result;
            categories = JsonConvert.DeserializeObject<List<Category>>(data);
            return categories;
        }

        public async Task<IEnumerable<Category>> GetCategories()
        {
            //return await _context.Categories.Where(c=>c.IsActive==true).ToListAsync();
            List<Category> categories = new List<Category>();
            HttpResponseMessage response = _http.GetAsync(baseAddress + "/CategoriesApi/GetCategories").Result;

            if (!response.IsSuccessStatusCode)
            {
                return new List<Category>();
            }

            string data = response.Content.ReadAsStringAsync().Result;
            categories = JsonConvert.DeserializeObject<List<Category>>(data);
            return categories;
        }

        public async Task<Category> GetCategoryById(int? id)
        {
            var category = new Category();
            HttpResponseMessage response = _http.GetAsync(baseAddress + "/CategoriesApi/" + id.ToString()).Result;

            if (!response.IsSuccessStatusCode)
            {
                return category;
            }

            string data = response.Content.ReadAsStringAsync().Result;
            category = JsonConvert.DeserializeObject<Category>(data);
            return category;
        }

        public async Task UpdateCategory(Category category)
        {
            string data = JsonConvert.SerializeObject(category);
            StringContent content = new StringContent(data, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _http.PutAsync(baseAddress + $"/CategoriesApi/{category.CategoryId}", content);
        }

        public bool CategoryExists(int id)
        {
            var category = new Category();
            HttpResponseMessage response = _http.GetAsync(baseAddress + "/CategoriesApi/" + id.ToString()).Result;

            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            string data = response.Content.ReadAsStringAsync().Result;
            category = JsonConvert.DeserializeObject<Category>(data);
            return (category==null)?false:true;
        }
    }
}

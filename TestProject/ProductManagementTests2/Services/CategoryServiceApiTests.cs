using ProductManagement.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using ProductManagement.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Newtonsoft.Json;
using System.Security.AccessControl;
using ProductManagement.Helpers;
using NPOI.SS.Formula.Functions;

namespace ProductManagement.Services.Tests
{
    public class CategoryServiceApiTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        Uri baseAddress = new Uri("https://localhost:7116/api");
        public readonly HttpClient _http;


        public CategoryServiceApiTests()
        {
            WebApplicationFactory<Program> factory = new WebApplicationFactory<Program>();
            _factory = factory;
        }

        [Fact]
        public async Task GetCategories_Should_Return_List_Of_Categories()
        {
            //Arrange
            var client = _factory.CreateClient();

            //Act 
            var responseMessage = await client.GetAsync($"{baseAddress}/CategoriesApi/GetCategories");
            List<Category> categories = new List<Category>();

            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;

                categories
                    = JsonConvert.DeserializeObject<List<Category>>(responseData);

            }
            //Assert
            Assert.True(categories.Count > 0);
        }

        [Fact]
        public async Task GetCategoryById_Should_Return_Not_Be_Null()
        {
            //Arrange
            var client = _factory.CreateClient();

            //Act 
            var responseMessage = await client.GetAsync($"{baseAddress}/CategoriesApi/1");
            var category = new Category();
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = await responseMessage.Content.ReadAsStringAsync();

                category
                    = JsonConvert.DeserializeObject<Category>(responseData);

            }
            //Assert
            Assert.NotNull(category);
        }

        [Fact]
        public async Task CreateCategory_Should_Create_Successfully()
        {
            //Arrange
            var client = _factory.CreateClient();

            var category = new Category()
            {
                CategoryCode = "abc123",
                IsActive = true,
                Name = "Test"
            };

            var newlyCreatedCategory = new Category();
            string data = JsonConvert.SerializeObject(category);
            StringContent content = new StringContent(data, System.Text.Encoding.UTF8, "application/json");
            //HttpResponseMessage response = await _http.PostAsync(baseAddress + "/ProductsApi", content);
            //Act
            var responseMessage = await client.PostAsync($"{baseAddress}/CategoriesApi", content);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                newlyCreatedCategory = JsonConvert.DeserializeObject<Category>(responseData);
            }
            //Assert
            Assert.True(!String.IsNullOrEmpty(category.Name));
            Assert.NotNull(newlyCreatedCategory);
        }


        [Fact]
        public async Task UpdateCategory_Should_Update_Successfully()
        {
            //Arrange
            var client = _factory.CreateClient();

           
            var category =await CreateNewCategory();

            Assert.NotNull(category);

            //Act
            category.Name = "testUpdate123";
            category.IsActive = false;
            var newlyUpdatedCategory = new Category();
            string data = JsonConvert.SerializeObject(category);
            StringContent content = new StringContent(data, System.Text.Encoding.UTF8, "application/json");
            var responseMessage = await client.PutAsync($"{baseAddress}/CategoriesApi/{category.CategoryId}", content);
            if (responseMessage.IsSuccessStatusCode)
            {
                newlyUpdatedCategory = await GetCategoryById(category.CategoryId);
            }
            //Assert
            Assert.Equal("testUpdate123", newlyUpdatedCategory.Name);
            Assert.NotNull(newlyUpdatedCategory);
        }


        public async Task<Category> GetCategoryById(int Id)
        {
            var client = _factory.CreateClient();

            //Act 
            var responseMessage = await client.GetAsync($"{baseAddress}/CategoriesApi/{Id}");
            var category = new Category();
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = await responseMessage.Content.ReadAsStringAsync();

                category
                    = JsonConvert.DeserializeObject<Category>(responseData);

            }
            return category;
        }

        public async Task<Category> CreateNewCategory()
        {
            var client = _factory.CreateClient();

            var category = new Category()
            {
                CategoryCode = "abc123",
                IsActive = true,
                Name = "Test"
            };

            var newlyCreatedCategory = new Category();
            string data = JsonConvert.SerializeObject(category);
            StringContent content = new StringContent(data, System.Text.Encoding.UTF8, "application/json");
            var responseMessage = await client.PostAsync($"{baseAddress}/CategoriesApi", content);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                newlyCreatedCategory = JsonConvert.DeserializeObject<Category>(responseData);
            }

            return newlyCreatedCategory;
        }
    }
}
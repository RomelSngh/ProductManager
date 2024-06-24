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
    public class ProductServiceApiTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        Uri baseAddress = new Uri("https://localhost:7116/api");
        public readonly HttpClient _http;

        public ProductServiceApiTests()
        {
            WebApplicationFactory<Program> factory = new WebApplicationFactory<Program>();
            _factory = factory;
        }

        [Fact]
        public async Task GetProducts_should_return_list_of_products()
        {
            //Arrange
            var client = _factory.CreateClient();

            //Act 
            var responseMessage = await client.GetAsync($"{baseAddress}/ProductsApi");
            List<Product> products = new List<Product>();

            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;

                products
                    = JsonConvert.DeserializeObject<List<Product>>(responseData);

            }
            //Assert
            Assert.True(products.Count > 0);
        }

        [Fact]
        public async Task GetProductById_should_return_not_be_null()
        {
            //Arrange
            var client = _factory.CreateClient();

            //Act 
            var responseMessage = await client.GetAsync($"{baseAddress}/ProductsApi/1");
            var product = new Product();
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = await responseMessage.Content.ReadAsStringAsync();

                product
                    = JsonConvert.DeserializeObject<Product>(responseData);

            }
            //Assert
            Assert.NotNull(product);
        }

        [Fact]
        public async Task CreateProduct_should_generate_product_code()
        {
            //Arrange
            var client = _factory.CreateClient();

            var product = new Product()
            { 
                CategoryId = 1,
                CategoryName = "Test",
                Price = 1,
                Description = "Test",
                Image = "Test",
                Name = "Test"
            };

            string data = JsonConvert.SerializeObject(product);
            StringContent content = new StringContent(data, System.Text.Encoding.UTF8, "application/json");
            //HttpResponseMessage response = await _http.PostAsync(baseAddress + "/ProductsApi", content);
            //Act
            var responseMessage = await client.PostAsync($"{baseAddress}/ProductsApi",content);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                product = JsonConvert.DeserializeObject<Product>(responseData);
            }
            //Assert
            Assert.True(!String.IsNullOrEmpty(product.ProductCode));
        }

        [Fact]
        public async Task UpdateProduct_Should_Update_Successfully()
        {
            //Arrange
            var client = _factory.CreateClient();


            var product = await CreateNewProduct();

            Assert.NotNull(product);

            //Act
            product.Name = "testUpdate123";

            var newlyUpdatedProduct = new Product();
            string data = JsonConvert.SerializeObject(product);
            StringContent content = new StringContent(data, System.Text.Encoding.UTF8, "application/json");
            var responseMessage = await client.PutAsync($"{baseAddress}/ProductsApi/{product.ProductId}", content);
            if (responseMessage.IsSuccessStatusCode)
            {
                newlyUpdatedProduct = await GetProductById(product.ProductId);
            }
            //Assert
            Assert.Equal("testUpdate123", newlyUpdatedProduct.Name);
            Assert.NotNull(newlyUpdatedProduct);
        }
        public async Task<Product> GetProductById(int Id)
        {
            var client = _factory.CreateClient();

            //Act 
            var responseMessage = await client.GetAsync($"{baseAddress}/ProductsApi/{Id}");
            var product = new Product();
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = await responseMessage.Content.ReadAsStringAsync();

                product
                    = JsonConvert.DeserializeObject<Product>(responseData);

            }

            return product;
        }

        public async Task<Product> CreateNewProduct()
        {
            var client = _factory.CreateClient();

            var product = new Product()
            {
                CategoryId = 1,
                CategoryName = "Test",
                Price = 1,
                Description = "Test",
                Image = "Test",
                Name = "Test"
            };

            string data = JsonConvert.SerializeObject(product);
            StringContent content = new StringContent(data, System.Text.Encoding.UTF8, "application/json");
            //HttpResponseMessage response = await _http.PostAsync(baseAddress + "/ProductsApi", content);
            //Act
            var responseMessage = await client.PostAsync($"{baseAddress}/ProductsApi", content);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                product = JsonConvert.DeserializeObject<Product>(responseData);
            }

            return product;
        }


    }
}
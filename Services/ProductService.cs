using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProductManagement.Contracts;
using ProductManagement.Data;
using ProductManagement.Models;
using System.Runtime.CompilerServices;

namespace ProductManagement.Services
{
    public class ProductService : IProductService
    {
        Uri baseAddress = new Uri("https://localhost:7116/api");
        public readonly HttpClient _http;

        public ProductService()
        {
            _http = new HttpClient();
            _http.BaseAddress = baseAddress;
        }

        public async Task DeleteProduct(Product product)
        {
            HttpResponseMessage response = await _http.DeleteAsync(baseAddress + $"/ProductsApi/{product.ProductId}");
        }

        public async Task<bool> ProductExists(int productId)
        {
            var product = new Product();
            HttpResponseMessage response =await  _http.GetAsync(baseAddress + "/ProductsApi/" + productId.ToString());

            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            string data = response.Content.ReadAsStringAsync().Result;
            product = JsonConvert.DeserializeObject<Product>(data);
            return (product == null) ? false : true;
        }

        public async Task UpdateProduct(Product product)
        {
            string data = JsonConvert.SerializeObject(product);
            StringContent content = new StringContent(data, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _http.PutAsync(baseAddress + $"/ProductsApi/{product.ProductId}", content);
        }

        public async Task<Product> GetProductById(int? productId)
        {
            var product = new Product();
            HttpResponseMessage response = _http.GetAsync(baseAddress + "/ProductsApi/" + productId.ToString()).Result;

            if (!response.IsSuccessStatusCode)
            {
                return product;
            }

            string data = response.Content.ReadAsStringAsync().Result;
            product = JsonConvert.DeserializeObject<Product>(data);
            return product;
        }

        public async Task<List<Product>> GetProducts()
        {
            List<Product> products = new List<Product>();
            HttpResponseMessage response =await _http.GetAsync(baseAddress + "/ProductsApi");

            if (!response.IsSuccessStatusCode)
            {
                return new List<Product>();
            }

            string data = response.Content.ReadAsStringAsync().Result;
            products = JsonConvert.DeserializeObject<List<Product>>(data);
            return products;
        }

        public async Task CreateProduct(Product product)
        {
            string data = JsonConvert.SerializeObject(product);
            StringContent content = new StringContent(data, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _http.PostAsync(baseAddress + "/ProductsApi", content);
        } 

    } 
}

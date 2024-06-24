using System.Collections.Generic;
using System.Threading.Tasks;
using ProductManagement.Controllers;
using ProductManagement.Models;
using ProductManagement.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using ProductManagement.Contracts;
using AutoMapper;

namespace ProductManagementTests2.Controllers
{
    public class ProductsControllerTests
    {
        private readonly Mock<IProductService> _productServiceMock;
        private readonly Mock<ICategoryService> _categoryServiceMock;
        private readonly Mock<IFileService> _fileServiceMock;
        private readonly Mock<IMapper> _mapper;
        private readonly ProductsController _controller;

        public ProductsControllerTests()
        {
            _productServiceMock = new Mock<IProductService>();
            _categoryServiceMock = new Mock<ICategoryService>();
            _fileServiceMock = new Mock<IFileService>();
            _mapper = new Mock<IMapper>();
            _controller = new ProductsController(_productServiceMock.Object,_categoryServiceMock.Object,_fileServiceMock.Object,_mapper.Object);
        }

        [Fact]
        public async Task Index_Should_Return_Single_Product()
        {
            // Arrange
            var viewNames = new[] { null, "Index" };
            var products = new List<Product>();
            var product = new Product()
            {
                CategoryId = 1,
                CategoryName = "Test",
                Price = 1,
                Description = "Test",
                Image = "Test",
                Name = "Test"
            };
            products.Add(product);

            _productServiceMock.Setup(c => c.GetProducts())
                                .ReturnsAsync(() => products);

            // Act
            var result = await _controller.Index() as ViewResult;
            var resultModel = (ProductListViewModel)result.Model; 
            // Assert
            Assert.True(resultModel != null);
            Assert.True(resultModel.Products.Count==1);
        }

        [Fact]
        public async Task ExportExcel_Should_Return_FileContentResult()
        {
            // Arrange
            var viewNames = new[] { null, "Index" };
            var products = new List<Product>();
            var product = new Product()
            {
                CategoryId = 1,
                CategoryName = "Test",
                Price = 1,
                Description = "Test",
                Image = "Test",
                Name = "Test"
            };
            products.Add(product);

            _productServiceMock.Setup(c => c.GetProducts())
                                .ReturnsAsync(() => products);

            // Act
            //_controller.ExportExcel().GetType().Name == "FileContentResult"
            var result = _controller.ExportExcel().GetType().Name as String;
            

            // Assert
            Assert.Equal("FileContentResult", result);
        }

       

    }
}

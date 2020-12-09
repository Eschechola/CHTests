using Xunit;
using Microsoft.AspNetCore.Mvc;
using CHTests.IntegrationTests.Fakes;
using Microsoft.EntityFrameworkCore;
using CHTests.Data.Context;
using CHTests.Data.Repositories;
using CHTests.Controllers;
using CHTests.Data.Entities;
using CHTests.Data.Interface;
using System.Threading.Tasks;
using System;

namespace CHTests.IntegrationTests.Products
{
    public class ProductsIntegrationTests
    {
        private FakeContext _context;
        private IProductRepository _productRepository;

        public ProductsIntegrationTests()
        {
            var options = new DbContextOptionsBuilder<TestsContext>()
                .UseInMemoryDatabase("Tests.db")
                .Options;

            _context = new FakeContext(options);
            _productRepository = new ProductRepository(_context);
        }

        [Fact]
        public async Task Values_Get_ReturnsOkResponse()
        {
            long productId = 85;
            await _productRepository.Add(new Product(productId, "Produto Teste", "Teste", 50));

            var controller = new ProductController(_productRepository);
            var response = await controller.Get(productId) as ObjectResult;

            Assert.Equal(200, response.StatusCode);
        }

        [Fact]
        public async Task Values_Get_ReturnsNoContentResult()
        {
            long productId = 123;

            var controller = new ProductController(_productRepository);
            var response = await controller.Get(productId) as ObjectResult;

            Assert.Equal(204, response.StatusCode);
        }

        [Fact]
        public async Task Values_Get_ReturnsInternalServerError()
        {
            long productId = 123;

            var controller = new ProductController(null);
            var response = await controller.Get(productId) as ObjectResult;

            Assert.Equal(500, response.StatusCode);
        }

        [Fact]
        public async Task Value_GetAll_ReturnsOkResponse()
        {
            await _productRepository.Add(new Product(new Random().Next(0, 999), "Produto Teste1", "Teste1", 50));
            await _productRepository.Add(new Product(new Random().Next(0, 999), "Produto Teste2", "Teste2", 50));
            await _productRepository.Add(new Product(new Random().Next(0, 999), "Produto Teste3", "Teste3", 50));

            var controller = new ProductController(_productRepository);
            var response = await controller.GetAll() as ObjectResult;

            Assert.Equal(200, response.StatusCode);
        }

        [Fact]
        public async Task Value_GetAll_ReturnsNoContentResult()
        {
            _productRepository = new ProductRepository(_context);

            var controller = new ProductController(_productRepository);
            var response = await controller.GetAll() as ObjectResult;

            Assert.Equal(204, response.StatusCode);
        }

        [Fact]
        public async Task Values_GetAll_ReturnsInternalServerError()
        {
            var controller = new ProductController(null);
            var response = await controller.GetAll() as ObjectResult;

            Assert.Equal(500, response.StatusCode);
        }

        [Fact]
        public async Task Values_Add_ReturnsOkResponse()
        {
            var product = new Product(new Random().Next(0, 999), "Produto Teste", "Teste", 10);
            
            _productRepository = new ProductRepository(_context);
            var controller = new ProductController(_productRepository);
            var response = await controller.Add(product) as ObjectResult;

            Assert.Equal(200, response.StatusCode);
        }

        [Fact]
        public async Task Values_Add_ReturnsInternalServerError()
        {
            var product = new Product(new Random().Next(0, 999), "Produto Teste", "Teste", 10);

            var controller = new ProductController(null);
            var response = await controller.Add(product) as ObjectResult;

            Assert.Equal(500, response.StatusCode);
        }
    }
}

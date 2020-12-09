using System;
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

namespace CHTests.IntegrationTests.Products
{
    public class ProductsIntegrationTests
    {
        private const int MIN_RANDOM_NUMBER = 0;
        private const int MAX_RANDOM_NUMBER = 999;

        #region Methods

        private DbContextOptions<TestsContext> GetContextOptions()
        {
            return new DbContextOptionsBuilder<TestsContext>()
                   .UseInMemoryDatabase("Tests.db")
                   .Options;
        }

        private FakeContext GetContext()
        {
            var contextOptions = GetContextOptions();
            return new FakeContext(contextOptions);
        }

        private IProductRepository GetProductRepository()
        {
            var context = GetContext();
            return new ProductRepository(context);
        }

        #endregion

        [Fact]
        public async Task Values_Get_ReturnsOkResponse()
        {
            long productId = new Random().Next(MIN_RANDOM_NUMBER, MAX_RANDOM_NUMBER);
            var productRepository = GetProductRepository();
            
            await productRepository.Add(new Product(productId, "Produto Teste", "Teste", 50));

            var controller = new ProductController(productRepository);
            var response = await controller.Get(productId) as ObjectResult;

            Assert.Equal(200, response.StatusCode);
        }

        [Fact]
        public async Task Values_Get_ReturnsNoContentResult()
        {
            long productId = 9998;
            var productRepository = GetProductRepository();

            var controller = new ProductController(productRepository);
            var response = await controller.Get(productId) as ObjectResult;

            Assert.Equal(204, response.StatusCode);
        }

        [Fact]
        public async Task Values_Get_ReturnsInternalServerError()
        {
            long productId = new Random().Next(MIN_RANDOM_NUMBER, MAX_RANDOM_NUMBER);

            var controller = new ProductController(null);
            var response = await controller.Get(productId) as ObjectResult;

            Assert.Equal(500, response.StatusCode);
        }

        [Fact]
        public async Task Value_GetAll_ReturnsOkResponse()
        {
            var productRepository = GetProductRepository();

            await productRepository.Add(new Product(new Random().Next(MIN_RANDOM_NUMBER, MAX_RANDOM_NUMBER), "Produto Teste1", "Teste1", 50));
            await productRepository.Add(new Product(new Random().Next(MIN_RANDOM_NUMBER, MAX_RANDOM_NUMBER), "Produto Teste2", "Teste2", 50));
            await productRepository.Add(new Product(new Random().Next(MIN_RANDOM_NUMBER, MAX_RANDOM_NUMBER), "Produto Teste3", "Teste3", 50));

            var controller = new ProductController(productRepository);
            var response = await controller.GetAll() as ObjectResult;

            Assert.Equal(200, response.StatusCode);
        }

        [Fact]
        public async Task Value_GetAll_ReturnsNoContentResult()
        {
            var productRepository = GetProductRepository();

            var controller = new ProductController(productRepository);
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
            var product = new Product(new Random().Next(MIN_RANDOM_NUMBER, MAX_RANDOM_NUMBER), "Produto Teste", "Teste", 10);
            var productRepository = GetProductRepository();

            var controller = new ProductController(productRepository);
            var response = await controller.Add(product) as ObjectResult;

            Assert.Equal(200, response.StatusCode);
        }

        [Fact]
        public async Task Values_Add_ReturnsInternalServerError()
        {
            var product = new Product(new Random().Next(MIN_RANDOM_NUMBER, MAX_RANDOM_NUMBER), "Produto Teste", "Teste", 10);

            var controller = new ProductController(null);
            var response = await controller.Add(product) as ObjectResult;

            Assert.Equal(500, response.StatusCode);
        }

        [Fact]
        public async Task Values_Update_ReturnsOkResponse()
        {
            var productId = new Random().Next(MIN_RANDOM_NUMBER, MAX_RANDOM_NUMBER);
            var product = new Product(productId, "Produto Teste", "Teste", 45);
            var productRepository = GetProductRepository();

            await productRepository.Add(product);

            product.Name = "Nome atualizado";

            var controller = new ProductController(productRepository);
            var response = await controller.Update(product) as ObjectResult;

            Assert.Equal(200, response.StatusCode);
        }

        [Fact]
        public async Task Values_Update_ReturnsInternalServerError()
        {
            var productId = new Random().Next(MIN_RANDOM_NUMBER, MAX_RANDOM_NUMBER);
            var product = new Product(productId, "Produto Teste", "Teste", 45);

            var controller = new ProductController(null);
            var response = await controller.Update(product) as ObjectResult;

            Assert.Equal(500, response.StatusCode);
        }


        [Fact]
        public async Task Values_Update_ReturnsBadRequest()
        {
            var productId = 9999;
            var product = new Product(productId, "Produto Teste", "Teste", 45);
            var productRepository = GetProductRepository();

            await productRepository.Add(product);

            product.Id = 1111;
            product.Name = "Nome atualizado";

            var controller = new ProductController(productRepository);
            var response = await controller.Update(product) as ObjectResult;

            Assert.Equal(400, response.StatusCode);
        }

        [Fact]
        public async Task Values_Remove_ReturnsOkResponse()
        {
            var productId = new Random().Next(MIN_RANDOM_NUMBER, MAX_RANDOM_NUMBER);
            var product = new Product(productId, "Produto Teste", "Teste", 45);

            var productRepository = GetProductRepository();

            await productRepository.Add(product);

            var controller = new ProductController(productRepository);
            var response = await controller.Remove(productId) as ObjectResult;

            Assert.Equal(200, response.StatusCode);
        }

        [Fact]
        public async Task Values_Remove_BadRequest()
        {
            var productId = 99999;
            var productRepository = GetProductRepository();

            var controller = new ProductController(productRepository);
            var response = await controller.Remove(productId) as ObjectResult;

            Assert.Equal(400, response.StatusCode);
        }


        [Fact]
        public async Task Values_Remove_ReturnsInternalServerError()
        {
            var productId = 99999; 

            var controller = new ProductController(null);
            var response = await controller.Remove(productId) as ObjectResult;

            Assert.Equal(500, response.StatusCode);
        }
    }
}

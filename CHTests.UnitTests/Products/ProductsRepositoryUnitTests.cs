using CHTests.Data.Context;
using CHTests.Data.Entities;
using CHTests.Data.Interface;
using CHTests.Data.Repositories;
using CHTests.UnitTests.Fakes;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Xunit;

namespace CHTests.UnitTests.Products
{
    public class ProductsRepositoryUnitTests
    {
        private const int MIN_RANDOM_NUMBER = 0;
        private const int MAX_RANDOM_NUMBER = 999;

        #region Methods

        private IProductRepository GetProductRepository()
        {
            //gera um banco pra cada teste!
            var contextOptions = new DbContextOptionsBuilder<TestsContext>()
                                 .UseInMemoryDatabase(Guid.NewGuid().ToString().Substring(0, 10) + ".db")
                                 .Options;

            var context = new FakeContext(contextOptions);
            return new ProductRepository(context);
        }

        #endregion

        [Fact]
        public async Task Repository_Add_ReturnsProductAdded()
        {
            var productId = new Random().Next(MIN_RANDOM_NUMBER, MAX_RANDOM_NUMBER);
            var product = new Product(productId, "Produto Teste", "Teste", 20);

            var productRepository = GetProductRepository();
            var productAdded = await productRepository.Add(product);

            Assert.Equal(productId, productAdded.Id);
        }


        [Fact]
        public async Task Repository_Add_ReturnsProductUpdated()
        {
            var productId = new Random().Next(MIN_RANDOM_NUMBER, MAX_RANDOM_NUMBER);
            var product = new Product(productId, "Produto Teste", "Teste", 20);

            var productRepository = GetProductRepository();
            await productRepository.Add(product);

            product.Name = "Nome atualizado!";

            var productUpdated = await productRepository.Update(product);

            Assert.True((productUpdated.Name == product.Name && productUpdated.Id == product.Id));
        }


        [Fact]
        public async Task Repository_Remove_ReturnsNullAfterRemove()
        {
            var productId = new Random().Next(MIN_RANDOM_NUMBER, MAX_RANDOM_NUMBER);
            var product = new Product(productId, "Produto Teste", "Teste", 20);

            var productRepository = GetProductRepository();
            await productRepository.Add(product);

            await productRepository.Remove(productId);

            var productSearch = await productRepository.Get(productId);

            Assert.True(productSearch == null);
        }

        [Fact]
        public async Task Repository_GetAll_ReturnAllProducts()
        {
            var productId1 = new Random().Next(MIN_RANDOM_NUMBER, MAX_RANDOM_NUMBER);
            var productId2 = new Random().Next(MIN_RANDOM_NUMBER, MAX_RANDOM_NUMBER);

            var product1 = new Product(productId1, "Produto Teste", "Teste", 20);
            var product2 = new Product(productId2, "Produto Teste2", "Teste2", 20);

            var productRepository = GetProductRepository();
            await productRepository.Add(product1);
            await productRepository.Add(product2);

            var allProducts = await productRepository.Get();

            Assert.True(allProducts.Count == 2);
        }


        [Fact]
        public async Task Repository_Get_ReturnProduct()
        {
            var productId = new Random().Next(MIN_RANDOM_NUMBER, MAX_RANDOM_NUMBER);
            var product = new Product(productId, "Produto Teste", "Teste", 20);

            var productRepository = GetProductRepository();
            await productRepository.Add(product);

            var productSearch = await productRepository.Get(productId);

            Assert.True(JsonConvert.SerializeObject(product) == JsonConvert.SerializeObject(productSearch));
        }
    }
}

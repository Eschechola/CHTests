using System;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using CHTests.Controllers;
using CHTests.Data.Entities;
using CHTests.Data.Interface;
using System.Threading.Tasks;
using Moq;
using System.Collections.Generic;

namespace CHTests.IntegrationTests.Products
{
    public class ProductsIntegrationTests
    {
        private readonly Mock<IProductRepository> _productRepositoryStub = new Mock<IProductRepository>();
        private const int MIN_RANDOM_NUMBER = 0;
        private const int MAX_RANDOM_NUMBER = 999;

        [Fact]
        public async Task Values_Get_ReturnsOkResponse()
        {
            //Arrange
            long productId = new Random().Next(MIN_RANDOM_NUMBER, MAX_RANDOM_NUMBER);

            var product = new Product(
                id: productId,
                name: "Karambit Doppler",
                description: "Skin CS:GO",
                mount: 2
            );

            var controller = new ProductController(_productRepositoryStub.Object);
            _productRepositoryStub
                .Setup(x => x.Get(It.IsAny<long>()))
                .ReturnsAsync(product);
            
            //Act
            var response = await controller.Get(productId) as ObjectResult;

            //Assert
            Assert.Equal(200, response.StatusCode);
        }

        [Fact]
        public async Task Values_Get_ReturnsNoContentResult()
        {
            //Arrange
            long productId = 9998;
            var controller = new ProductController(_productRepositoryStub.Object);
            _productRepositoryStub
                .Setup(x => x.Get(It.IsAny<long>()))
                .ReturnsAsync((Product)null);

            //Act
            var response = await controller.Get(productId) as ObjectResult;

            //Assert
            Assert.Equal(204, response.StatusCode);
        }

        [Fact]
        public async Task Values_Get_ReturnsInternalServerError()
        {
            //Arrange
            long productId = new Random().Next(MIN_RANDOM_NUMBER, MAX_RANDOM_NUMBER);
            var controller = new ProductController(null);
            
            //Act
            var response = await controller.Get(productId) as ObjectResult;

            //Assert
            Assert.Equal(500, response.StatusCode);
        }

        [Fact]
        public async Task Value_GetAll_ReturnsOkResponse()
        {
            //Arrange
            List<Product> productList = new List<Product>
            {
                new Product(new Random().Next(MIN_RANDOM_NUMBER, MAX_RANDOM_NUMBER), "Butterfly Safira", "Safira", 30),
                new Product(new Random().Next(MIN_RANDOM_NUMBER, MAX_RANDOM_NUMBER), "Karambit Doppler", "Ruby", 20),
                new Product(new Random().Next(MIN_RANDOM_NUMBER, MAX_RANDOM_NUMBER), "M9 Bayonet Doppler", "Esmerald", 10)
            };

            var controller = new ProductController(_productRepositoryStub.Object);
            _productRepositoryStub
                .Setup(x => x.Get().Result)
                .Returns(productList);
            
            //Act
            var response = await controller.GetAll() as ObjectResult;

            //Assert
            Assert.Equal(200, response.StatusCode);
        }

        [Fact]
        public async Task Value_GetAll_ReturnsNoContentResult()
        {
            //Arrange
            var controller = new ProductController(_productRepositoryStub.Object);
            _productRepositoryStub
                .Setup(x => x.Get().Result)
                .Returns(new List<Product>());

            //Act
            var response = await controller.GetAll() as ObjectResult;

            //Assert
            Assert.Equal(204, response.StatusCode);
        }

        [Fact]
        public async Task Values_GetAll_ReturnsInternalServerError()
        {
            //Arrange
            var controller = new ProductController(null);

            //Act
            var response = await controller.GetAll() as ObjectResult;

            //Assert
            Assert.Equal(500, response.StatusCode);
        }

        [Fact]
        public async Task Values_Add_ReturnsOkResponse()
        {
            //Arrange
            var product = new Product(
                id: new Random().Next(MIN_RANDOM_NUMBER, MAX_RANDOM_NUMBER),
                name: "Talon Ultraviolet",
                description: "Rare Factory New UltraViolet",
                mount: 10
            );
            
            var controller = new ProductController(_productRepositoryStub.Object);
            _productRepositoryStub
                .Setup(x => x.Add(It.IsAny<Product>()))
                .ReturnsAsync(product);
            
            //Act
            var response = await controller.Add(product) as ObjectResult;

            //Assert
            Assert.Equal(200, response.StatusCode);
        }

        [Fact]
        public async Task Values_Add_ReturnsInternalServerError()
        {
            var product = new Product(
                id: new Random().Next(MIN_RANDOM_NUMBER, MAX_RANDOM_NUMBER),
                name: "Survival Case Hardned",
                description: "Blue Gem",
                mount: 10
            );

            var controller = new ProductController(null);
            var response = await controller.Add(product) as ObjectResult;

            Assert.Equal(500, response.StatusCode);
        }

        [Fact]
        public async Task Values_Update_ReturnsOkResponse()
        {
            var productId = new Random().Next(MIN_RANDOM_NUMBER, MAX_RANDOM_NUMBER);
            var product = new Product(
                id: productId,
                name: "Paracord Fad3",
                description: "Fade Knife!",
                mount: 45);

            var productUpdated = product;
            productUpdated.Name = "Paracord Fade";

            var controller = new ProductController(_productRepositoryStub.Object);
            
            _productRepositoryStub
                .Setup(x => x.Get(It.IsAny<long>()))
                .ReturnsAsync(product);

            _productRepositoryStub
                .Setup(x => x.Update(It.IsAny<Product>()))
                .ReturnsAsync(productUpdated);

            //Act
            var response = await controller.Update(productUpdated) as ObjectResult;

            //Assert
            Assert.Equal(200, response.StatusCode);
        }

        [Fact]
        public async Task Values_Update_ReturnsInternalServerError()
        {
            //Act
            var productId = new Random().Next(MIN_RANDOM_NUMBER, MAX_RANDOM_NUMBER);
            var product = new Product(
                id: productId,
                name: "Classic Crimson Web",
                description: "Spider man knife??",
                mount: 45);

            var controller = new ProductController(null);
            
            //Arrange
            var response = await controller.Update(product) as ObjectResult;

            //Assert
            Assert.Equal(500, response.StatusCode);
        }


        [Fact]
        public async Task Values_Update_ReturnsBadRequest()
        {
            //Arrange
            var productId = 9999;
            var product = new Product(
                id: productId,
                name: "Bayonet Doppler",
                description: "Black Perl",
                mount: 45);

            product.Id = 1111;
            product.Name = "Bayonet Doppl3r";

            var controller = new ProductController(_productRepositoryStub.Object);

            _productRepositoryStub
                .Setup(x => x.Get(productId))
                .ReturnsAsync((Product)null);

            //Act
            var response = await controller.Update(product) as ObjectResult;

            //Assert
            Assert.Equal(400, response.StatusCode);
        }

        [Fact]
        public async Task Values_Remove_ReturnsOkResponse()
        {
            //Arrange
            var productId = new Random().Next(MIN_RANDOM_NUMBER, MAX_RANDOM_NUMBER);
            var product = new Product(
                id: productId,
                name: "Skeleton Safari Mesh",
                description: "Dirty knife??",
                mount: 45);

            var controller = new ProductController(_productRepositoryStub.Object);
            _productRepositoryStub
                .Setup(x => x.Get(productId))
                .ReturnsAsync(product);

            //Act
            var response = await controller.Remove(productId) as ObjectResult;

            //Assert
            Assert.Equal(200, response.StatusCode);
        }

        [Fact]
        public async Task Values_Remove_BadRequest()
        {
            //Arrange
            var productId = 99999;
            var controller = new ProductController(_productRepositoryStub.Object);
            _productRepositoryStub
                .Setup(x => x.Get(productId))
                .ReturnsAsync((Product)null);

            //Act
            var response = await controller.Remove(productId) as ObjectResult;

            //Assert
            Assert.Equal(400, response.StatusCode);
        }


        [Fact]
        public async Task Values_Remove_ReturnsInternalServerError()
        {
            //Arrange
            var productId = 99999; 
            var controller = new ProductController(null);
            
            //Act
            var response = await controller.Remove(productId) as ObjectResult;

            //Assert
            Assert.Equal(500, response.StatusCode);
        }
    }
}

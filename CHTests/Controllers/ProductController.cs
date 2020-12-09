using System;
using System.Threading.Tasks;
using CHTests.Data.Entities;
using CHTests.Data.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CHTests.Controllers
{
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpPost]
        [Route("/api/v1/product/add")]
        public async Task<IActionResult> Add([FromBody]Product product)
        {
            try
            {
                return Ok(await _productRepository.Add(product));
            }
            catch (Exception)
            {
                return StatusCode(500, "Ocorreu algum erro interno na aplicação!");
            }
        }

        [HttpPut]
        [Route("/api/v1/product/update")]
        public async Task<IActionResult> Update([FromBody] Product product)
        {
            try
            {
                var productExists = await _productRepository.Get(product.Id);

                if (productExists == null)
                    return StatusCode(400, ("Nenhum produto foi encontrado com o Id" + product.Id));

                var productUpdated = await _productRepository.Update(product);
                return Ok(productUpdated);
            }
            catch (Exception)
            {
                return StatusCode(500, "Ocorreu algum erro interno na aplicação!");
            }
        }

        [HttpDelete]
        [Route("/api/v1/product/remove")]
        public async Task<IActionResult> Remove([FromQuery]long id)
        {
            try
            {
                var productExists = await _productRepository.Get(id);

                if (productExists == null)
                    return BadRequest("O produto informado não foi encontrado!");

                await _productRepository.Remove(id);
                return Ok("Usuário deletado com sucesso!");
            }
            catch (Exception)
            {
                return StatusCode(500, "Ocorreu algum erro interno na aplicação!");
            }
        }

        [HttpGet]
        [Route("/api/v1/product/get")]
        public async Task<IActionResult> Get([FromQuery]long id)
        {
            try
            {
                var product = await _productRepository.Get(id);

                if (product != null)
                    return Ok(product);
                else
                    return StatusCode(204, ("Nenhum produto encontrado com o Id +", id));
            }
            catch (Exception)
            {
                return StatusCode(500, "Ocorreu algum erro interno na aplicação!");
            }
        }


        [HttpGet]
        [Route("/api/v1/product/get-all")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var products = await _productRepository.Get();

                if (products.Count > 0)
                    return Ok(products);
                else
                    return StatusCode(204, "Nenhum produto encontrado na base de dados!");
            }
            catch (Exception)
            {
                return StatusCode(500, "Ocorreu algum erro interno na aplicação!");
            }
        }
    }
}

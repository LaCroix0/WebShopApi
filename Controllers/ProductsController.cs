using Microsoft.AspNetCore.Mvc;
using WebShopApi.Models;
using WebShopApi.Models.DTO;
using WebShopApi.Models.Repository;

namespace WebShopApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : Controller
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IProductRepository _productRepository;

        public ProductsController(ILogger<ProductsController> logger, IProductRepository productRepository)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string? orderBy)
        {
            var OrderByValues = new HashSet<string>() { "name", "description", "category", "serialnumber", "producer" };
            orderBy ??= "name";

            if (!OrderByValues.Contains(orderBy.ToLower()))
            {
                return BadRequest();
            }

            var products = new List<Product>();
            await _productRepository.Get(products, orderBy);
            return Ok(products);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            Product product = null;
            await _productRepository.Get(id, product);
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Product product)
        {
            await _productRepository.Post(new Product()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Category = product.Category,
                SerialNumber = product.SerialNumber,
                Producer = product.Producer
            });
            return Created($"/api/animals/{product.Id}", product);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _productRepository.Delete(id);
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, AddProduct product)
        {
            await _productRepository.Put(id, new Product()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Category = product.Category,
                SerialNumber = product.SerialNumber,
                Producer = product.Producer
            });
            return NoContent();
        }
    }
}

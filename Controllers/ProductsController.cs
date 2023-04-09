using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebShopApi.Data;
using WebShopApi.Models;
using WebShopApi.Models.DTO;
using WebShopApi.Models.Repository;

namespace WebShopApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IProductRepository _productRepository;
        private readonly DataContext _context;

        public ProductsController(ILogger<ProductsController> logger, IProductRepository productRepository, DataContext context)
        {
            _logger = logger;
            _productRepository = productRepository;
            _context = context;
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


        [HttpPost("addProduct")]
        public async Task<IActionResult> Post(ProductDTO productDto)
        {
            await _productRepository.Post(productDto);
            return Created($"/api/products/{productDto.Name}", productDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _productRepository.Delete(id);
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, ProductDTO productDto)
        {
            await _productRepository.Put(id, new Product()
            {
                Name = productDto.Name,
                Description = productDto.Description,
                Category = productDto.Category,
                SerialNumber = productDto.SerialNumber,
                Producer = productDto.Producer
            });
            return NoContent();
        }
    }
}

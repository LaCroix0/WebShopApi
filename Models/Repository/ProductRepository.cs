using System.Data.SqlClient;
using WebShopApi.Data;
using WebShopApi.Models.DTO;

namespace WebShopApi.Models.Repository
{
    public interface IProductRepository
    {
        Task Get(List<Product> products, string? orderBy);
        Task Post(ProductDTO productDto);
        Task Delete(int it);
        Task Put(int id, Product product);
    }


    public class ProductRepository : IProductRepository
    {
        private readonly IConfiguration _configuration;
        private readonly DataContext _context;
        public ProductRepository(IConfiguration configuration, DataContext context)
        {
            _configuration = configuration;
            _context = context;
        }
        public async Task Get(List<Product> products, string? orderBy)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("Default")))
            {
                var command = connection.CreateCommand();
                command.CommandText =
                    $"SELECT * FROM Products order by {orderBy}";
                await connection.OpenAsync();
                var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    products.Add(new Product()
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Description = reader.GetValue(2) == DBNull.Value ? null : reader.GetString(2),
                        Category = reader.GetString(3),
                        SerialNumber = reader.GetInt32(4),
                        Producer = reader.GetString(5)
                    });
                }
            }
        }

        public async Task Post(ProductDTO productDto)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("Default")))
            {
                var product = new Product()
                {
                    Name = productDto.Name,
                    Description = productDto.Description,
                    Category = productDto.Category,
                    SerialNumber = productDto.SerialNumber,
                    Producer = productDto.Producer
                };
                _context.Products.Add(product);
                await _context.SaveChangesAsync();

            }
        }

        public async Task Delete(int id)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("Default")))
            {
                Product product = _context.Products.Find(id);
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }

        public async Task Put(int id, Product product)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("Default")))
            {
                var command = connection.CreateCommand();
                command.CommandText = 
                    "Update Products set username = @1, description = @2, category = @3, serialnumber = @4, producer = @5 where id = @6";
                command.Parameters.AddWithValue("@1", product.Name);
                command.Parameters.AddWithValue("@2", product.Description is null ? DBNull.Value : product.Description);
                command.Parameters.AddWithValue("@3", product.Category);
                command.Parameters.AddWithValue("@4", product.SerialNumber);
                command.Parameters.AddWithValue("@5", product.Producer);
                command.Parameters.AddWithValue("@6", id);
                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
        }
    }
}

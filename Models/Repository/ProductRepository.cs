using System.Data.SqlClient;

namespace WebShopApi.Models.Repository
{
    public interface IProductRepository
    {
        Task Get(List<Product> products, string? orderBy);
        Task Get(int id, Product product);
        Task Post(Product product);
        Task Delete(int it);
        Task Put(int id, Product product);
    }


    public class ProductRepository : IProductRepository
    {
        private readonly IConfiguration _configuration;
        public ProductRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task Get(List<Product> products, string? orderBy)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("Default")))
            {
                var command = connection.CreateCommand();
                command.CommandText =
                    $"SELECT * FROM Product order by {orderBy}";
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

        public async Task Get(int id, Product product)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("Default")))
            {
                var command = connection.CreateCommand();
                command.CommandText =
                    $"SELECT * FROM Product WHERE id={id}";
                await connection.OpenAsync();
                var reader = await command.ExecuteReaderAsync();
                product.Id = reader.GetInt32(0);
                product.Name = reader.GetString(1);
                product.Description = reader.GetValue(2) == DBNull.Value ? null : reader.GetString(2);
                product.Category = reader.GetString(3);
                product.SerialNumber = reader.GetInt32(4);
                product.Producer = reader.GetString(5);
            }
        }

        public async Task Post(Product product)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("Default")))
            {
                var command = connection.CreateCommand();
                command.CommandText =
                    "Insert into Product (id, name, description, category, serialnumber, producer) values (@1, @2, @3, @4, @5, @6)";
                command.Parameters.AddWithValue("@1", product.Id);
                command.Parameters.AddWithValue("@2", product.Name);
                command.Parameters.AddWithValue("@3", product.Description is null ? DBNull.Value : product.Description);
                command.Parameters.AddWithValue("@4", product.Category);
                command.Parameters.AddWithValue("@5", product.SerialNumber);
                command.Parameters.AddWithValue("@6", product.Producer);
                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task Delete(int id)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("Default")))
            {
                var command = connection.CreateCommand();
                command.CommandText =
                    "Delete from Product where id = @1";
                command.Parameters.AddWithValue("@1", id);
                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task Put(int id, Product product)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("Default")))
            {
                var command = connection.CreateCommand();
                command.CommandText = 
                    "Update Product set name = @1, description = @2, category = @3, serialnumber = @4, producer = @5 where id = @6";
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

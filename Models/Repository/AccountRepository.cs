using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.EntityFrameworkCore;
using WebShopApi.Data;
using WebShopApi.Models.DTOs;

namespace WebShopApi.Models.Repository
{
    public interface IAccountsRepository
    {
        Task Get(List<DisplayUserDTO> users);
        Task Register(RegisterDTO registerDto);
        Task<Account> Login(LoginDTO loginDto);
        Task<bool> UserExists(string name);
    }
    public class AccountRepository : IAccountsRepository
    {
        private readonly IConfiguration _configuration;
        private readonly DataContext _context;
        private readonly ITokenRepository _tokenRepository;
        public AccountRepository(IConfiguration configuration, DataContext dataContext)
        {
            _context = dataContext;
            _configuration = configuration;
        }

        public async Task Get(List<DisplayUserDTO> users)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("Default")))
            {
                var command = connection.CreateCommand();
                command.CommandText =
                    "SELECT * FROM Users";
                await connection.OpenAsync();
                var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    users.Add(new DisplayUserDTO()
                    {
                        id = reader.GetInt32(0),
                        name = reader.GetString(1),
                        email = reader.GetString(4),
                        role = reader.GetString(5)
                    });
                }
            }
        }
        public async Task Register(RegisterDTO registerDto)
        {
            using var hmac = new HMACSHA512();
            Account account = new Account()
            {
                Name = registerDto.name.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.password)),
                PasswordSalt = hmac.Key,
                Email = registerDto.email,
                Role = "User"
            };
            
            _context.Users.Add(account);
            await _context.SaveChangesAsync();

        }

        [HttpPost("login")]
        public async Task<Account> Login(LoginDTO loginDto)
        {
            var account = await _context.Users.SingleOrDefaultAsync(x => x.Name == loginDto.name);

            var hmac = new HMACSHA512(account.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.password));
            
            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != account.PasswordHash[i]) throw new Exception("Wrong password");
            }

            return account;
        }

        public async Task<bool> UserExists(string name)
        {
            return await _context.Users.AnyAsync(x => x.Name == name.ToLower());
        }
    }
}

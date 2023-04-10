using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
using System.Linq.Expressions;
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
        Task<DisplayUserDTO> Register(RegisterDTO registerDto);
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
                var accounts = await _context.Users.ToListAsync();
                foreach (var account in accounts)
                {
                    users.Add(new DisplayUserDTO()
                    {
                        id = account.Id,
                        name = account.Name,
                        email = account.Email,
                        role = account.Role
                    });
                }
            }
        }
        public async Task<DisplayUserDTO> Register(RegisterDTO registerDto)
        {
            using var hmac = new HMACSHA512();
            Account account = new Account()
            {
                Name = registerDto.username.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.password)),
                PasswordSalt = hmac.Key,
                Email = registerDto.email,
                Role = "User"
            };

            
            _context.Users.Add(account);
            await _context.SaveChangesAsync();

            return new DisplayUserDTO()
            {
                id = account.Id,
                name = account.Name,
                email = account.Email,
                role = account.Role
            }; ;
        }

        [HttpPost("login")]
        public async Task<Account> Login(LoginDTO loginDto)
        {
            if (await _context.Users.SingleOrDefaultAsync(x => x.Name == loginDto.username) == null)
            {
                return null;
            }
            var account = await _context.Users.SingleOrDefaultAsync(x => x.Name == loginDto.username);

            var hmac = new HMACSHA512(account.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.password));
            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != account.PasswordHash[i]) return null;
            }

            return account;
        }

        public async Task<bool> UserExists(string name)
        {
            return await _context.Users.AnyAsync(x => x.Name == name.ToLower());
        }
    }
}

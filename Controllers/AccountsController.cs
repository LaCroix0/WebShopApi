using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebShopApi.Data;
using WebShopApi.Models;
using WebShopApi.Models.DTOs;
using WebShopApi.Models.Repository;

namespace WebShopApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : Controller
    {
        private readonly DataContext _context;
        private readonly ILogger<AccountsController> _logger;
        private readonly IAccountsRepository _accountRepository;
        private readonly ITokenRepository _tokenRepository;

        public AccountsController(
            DataContext context, ILogger<AccountsController> logger,
            IAccountsRepository accountRepository, ITokenRepository tokenRepository)
        {
            _logger = logger;
            _accountRepository = accountRepository;
            _context = context;
            _tokenRepository = tokenRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            List<DisplayUserDTO> users = new List<DisplayUserDTO>();
            await _accountRepository.Get(users);
            return Ok(users);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO registerDto)
        {
            if (await _accountRepository.UserExists(registerDto.name)) return BadRequest("Username is taken");
            await _accountRepository.Register(registerDto);

            return Created($"/api/accounts/{registerDto.name}", registerDto);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO loginDto)
        {
            var user = await _accountRepository.Login(loginDto);
            return Ok(user);
        }


    }
}

using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;
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
        public async Task<ActionResult> Get()
        {
            List<DisplayUserDTO> users = new List<DisplayUserDTO>();
            await _accountRepository.Get(users);
            return Ok(users);
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDto)
        {
            if (await _accountRepository.UserExists(registerDto.name)) return BadRequest("Username is taken");
            await _accountRepository.Register(registerDto);
            var user = await _accountRepository.Register(registerDto);

            // chyba do wyjebania elo
            var userDto = new UserDTO()
            {
                username = user.Name,
                Token = _tokenRepository.CreateToken(user)
            };

            return Created($"/api/accounts/{userDto.username}", userDto);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDto)
        {
            var user = await _accountRepository.Login(loginDto);
            var userDto = new UserDTO()
            {
                username = user.Name,
                Token = _tokenRepository.CreateToken(user)
            };
            
            return Ok(userDto);
        }


    }
}

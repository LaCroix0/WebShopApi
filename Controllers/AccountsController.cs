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
        private readonly IAccountsRepository _accountRepository;
        private readonly ITokenRepository _tokenRepository;

        public AccountsController( IAccountsRepository accountRepository, ITokenRepository tokenRepository)
        {
            _accountRepository = accountRepository;
            _tokenRepository = tokenRepository;
        }

        [HttpGet("/GetAll")]
        public async Task<ActionResult> Get()
        {
            List<DisplayUserDTO> users = new List<DisplayUserDTO>();
            await _accountRepository.Get(users);
            return Ok(users);
        }

        [HttpPost("/register")]
        [AllowAnonymous]
        public async Task<ActionResult> Register(RegisterDTO registerDto)
        {
            if (await _accountRepository.UserExists(registerDto.name)) return BadRequest("Username is taken");
            await _accountRepository.Register(registerDto);
            var user = await _accountRepository.Register(registerDto);


            return Created($"/api/accounts/{user.name}", user);
        }

        [HttpPost("/login")]
        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginDTO loginDto)
        {
            var user = await _accountRepository.Login(loginDto);
            if (user == null) return Unauthorized("Invalid username or password");
            var userDto = new UserDTO()
            {
                username = user.Name,
                Token = _tokenRepository.CreateToken(user)
            };
            
            return Ok(userDto);
        }


    }
}

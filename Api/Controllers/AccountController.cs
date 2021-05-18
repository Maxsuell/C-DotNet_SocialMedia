using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Api.Data;
using Api.Dto;
using Api.Entities;
using Api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    public class AccountController : BaseApiController
    {
        // Declaração de variaveis e injeção de dependencia 
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;

        public AccountController(DataContext context, ITokenService tokenService)
        {
            _tokenService = tokenService;

            _context = context;
        }

        // Metodo http do tipo post, função de registro
        //Link url/api/register
        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await UserExists(registerDto.Username)) return BadRequest("Username is taken");

            using var hmac = new HMACSHA512();

            var user = new AppUser
            {
                UserName = registerDto.Username.ToLower(),
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                passwordSalt = hmac.Key
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new UserDto
            {
                UserName = user.UserName,
                token = _tokenService.CreateToken(user)
            };

        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> login(LoginDto loginDto)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == loginDto.Username.ToLower());

            if (user == null) return Unauthorized("Invalid Username");

            using var hmac = new HMACSHA512(user.passwordSalt);

            var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i = 0; i < computeHash.Length; i++)
            {
                if (computeHash[i] != user.passwordHash[i]) return Unauthorized("Invalid password");
            }

            return new UserDto
            {
                UserName = user.UserName,
                token = _tokenService.CreateToken(user)
            };
        }

        private async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync(x => x.UserName == username.ToLower());
        }
    }
}
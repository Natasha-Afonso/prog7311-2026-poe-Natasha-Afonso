using MedicMoveLogisticsAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MedicMoveLogisticsAPI.Data;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace MedicMoveLogisticsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;

        public AccountsController(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        //Register new account
        [HttpPost("register")]
        public async Task<IActionResult> Register(Register dto)
        {
            if (await _context.Accounts.AnyAsync(a => a.Email == dto.Email))
            {
                return BadRequest(new { message = "Email is already registered" });
            }

            // Map data from DTO to Account database model
            var account = new Accounts
            {
                Email = dto.Email,
                Password = dto.Password,
                Role = dto.Role, // e.g "Admin" or "Client" or Driver
                ClientId = dto.ClientId == 0 ? null : dto.ClientId // Set ClientId to null if it's 0
            };

            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Registration successful" });
        }

        //Login existing account
        [HttpPost("login")]
        public IActionResult Login(Login login)
        {
            //Find user in the database with matching email and password
            var account = _context.Accounts.FirstOrDefault(a => a.Email == login.Email && a.Password == login.Password);
            if (account == null)
            {
                return Unauthorized(new { message = "Invalid email or password" });
            }

            //Generate JWT claims
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, account.ID.ToString()),
                new Claim(ClaimTypes.Email, account.Email),
                new Claim(ClaimTypes.Role, account.Role),
                new Claim("ClientId", account.ClientId?.ToString() ?? "0") // Include ClientId as a claim, use "null" if it's null
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                 issuer: _config["Jwt:Issuer"],
                 audience: _config["Jwt:Audience"],
                 claims: claims,
                 expires: DateTime.Now.AddHours(2),
                 signingCredentials: creds
             );

            //Send the token back to the interface
            return Ok(new 
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo,
                Role = account.Role,
                Email = account.Email,
            });
        }
    }
}
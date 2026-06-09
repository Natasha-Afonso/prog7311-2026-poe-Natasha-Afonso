using MedicMoveLogisticsAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MedicMoveLogisticsAPI.Data;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;

//Emeris School of Computer Science, 2025
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
/** 
References
Chandra, S. 2026. JWT Authentication in ASP.NET Core: A complete step-bystey guide. (Version 10.0) [Source Code]. Available at: <https://medium.com/@Rajdip27/jwt-authentication-in-asp-net-core-a-complete-step-by-step-guide-94c955d907da> [Accessed 7 June 2026]
CodeWithGopi. 2025. Create and Consume Web API in ASP.NET Core MVC | Full CRUD Operatins & API Consumption. (Version 10.0) [Source Code]. Available at:<https://youtu.be/knTcwvFWOQM?si=b3q3ksLg6HNnBjDg> [Accessed 4 June 2026]
Emad, C. 2024. ASP.NET Core Web API with JWT (JSON Web Token) authentication. (Version 10.0) [Source Code]. Available at: <https://github.com/carolemad965/ASP.NET-Core-Web-API-with-JWT-Authentication> [Accessed 7 June 2026]
Emeris School of Computer Science. 2025. PROG7311 Docker part 1 - Getting started. (Version 10.0) [Source Code]. Available at: <https://youtu.be/ldfC-rTzHPY?si=znsmPz-ZSPaY-4Ii> [Accessed 5 June 2026]
Emeris School of Computer Science. 2025. PROG7311 Docker part 2- connecting between the two!. (Version 10.0) [Source Code]. Available at: <https://youtu.be/_0EhC3WauAc?si=nEazkCpBTV1YpHBp> [Accessed 5 June 2026]
Emeris School of Computer Science. 2025. PROG7311 Docker part 3- databases in docker. (Version 10.0) [Source Code]. Available at: <https://youtu.be/6tYEbngWY-E?si=10rSEEpfSNL5Kg8k> [Accessed 5 June 2026]
Emeris School of Computer Science. 2025. PROG7311 Docker part 4- connecting to database from the API.(Version 10.0) [Source Code]. Available at: <https://youtu.be/_PwpPMoeqzw?si=Q0ELlPZdN42dFU9q> [Accessed 5 June 2026]
Emeris School of Computer Science. 2025. PROG7311 Docker part 5- connecting them all together- DB -API -MVC. (Version 10.0) [Source Code]. Available at: <https://youtu.be/YlIajRMdLPk?si=D75XZKO-L6nQhR-3> [Accessed 5 June 2026]
Microsoft Learn. 2026. Parameter Binding in Minimal API apps. (Version 10)[source code]. Available at: <https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis/parameter-binding?view=aspnetcore-10.0> [Accessed 5 June 2026]
Microsoft Learn. 2023. Use IHttpClientFactory to implement resilient HTTP requests. (Version 10)[source code]. Available at: <https://learn.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests> [Accessed 5 June 2026]
Verma, S. 2024. Model Binding in ASP.NET Core. (Version 10)[source code]. Available at: <https://www.c-sharpcorner.com/article/model-binding-in-asp-net-core2/> [Accessed 5 June 2026]           
Saha, D.2024. Implement Cascading Dropdown In ASP.NET Core MVC Application Using InMemory DB.(Version 10)[source code]. Available at: < https://www.c-sharpcorner.com/article/implement-cascading-dropdown-in-asp-net-core-mvc-application-using-inmemory-db/ > [Accessed 7 June 2026]
Williams, R.2026.SEND IT QUICK COURIER SERVICE.(Version 10)[source code]. Available at: < https://codepen.io/dasia16/pen/azvEBBp > [Accessed 5 June 2026]
*/

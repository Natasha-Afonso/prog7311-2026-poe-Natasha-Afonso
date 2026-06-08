using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MedicMoveLogisticsMVC.Services;
using MedicMoveLogisticsMVC.Models;

namespace MedicMoveLogisticsMVC.Controllers
{
    public class AccountsController : Controller
    {
        private readonly AuthService _authService;

        public AccountsController(AuthService authService)
        {
            _authService = authService;
        }
        [HttpGet]
        [Route("Accounts")]
        [Route("Accounts/Login")]
        public IActionResult Login()
        {
            return View(new LoginView());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginView login)
        {
            if (!ModelState.IsValid)
            {
                return View(login);
            }

            var authResult = await _authService.LoginAsync(login);

            if (authResult == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt. Please check your email address or password.");
                return View(login);
            }


            HttpContext.Session.SetString("JWTToken", authResult.Token);
            HttpContext.Session.SetString("AccountRole", authResult.Role);
            HttpContext.Session.SetString("UserEmail", login.Email);

if (authResult.ClientId.HasValue)
            {
                HttpContext.Session.SetInt32("AccountClientId",authResult.ClientId.Value);
            }

            if (authResult.Role == "Admin")
            {
                return RedirectToAction("Index", "Admin");
            }
            else if (authResult.Role == "Client")
            {
                return RedirectToAction("Index", "Client");
            }
            
        ModelState.AddModelError(string.Empty,"Unauthorized role configuration");
        return View(login);
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
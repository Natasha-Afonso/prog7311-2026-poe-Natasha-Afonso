using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MedicMoveLogisticsMVC.Services;
using MedicMoveLogisticsMVC.Models;

namespace MedicMoveLogisticsMVC.Controllers
{
    public class AccountsController : Controller
    {
        private readonly AuthService _authService;
        private readonly ClientService _clientService;

        public AccountsController(AuthService authService, ClientService clientService)
        {
            _authService = authService;
            _clientService = clientService;
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



            if (authResult.Role == "Admin")
            {
                return RedirectToAction("Index", "Admin");
            }else if (authResult.Role == "Driver")
            {
                return RedirectToAction("Index", "ServiceRequests");
            }
            else if (authResult.Role == "Client")
            {
                try
                {
                    var allClients = await _clientService.GetAllClientsAsync(authResult.Token);
                    var currentClient = allClients.FirstOrDefault(c => c.ContactDetails == login.Email);
                    if (currentClient != null)
                    {
                        HttpContext.Session.SetInt32("AccountClientId", currentClient.Id);
                        return RedirectToAction("Index", "Client");

                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Your account is verified but no client");
                        return View(login);
                    }
                }
                catch (Exception ex)
                {

                    ModelState.AddModelError(string.Empty, $"Unauthorized role configuration: {ex.Message}");
                    return View(login);
                }
            }
            ModelState.AddModelError(string.Empty, "Unauthorized role configuration");
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
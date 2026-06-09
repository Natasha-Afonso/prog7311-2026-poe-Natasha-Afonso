using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using MedicMoveLogisticsMVC.Services;
using MedicMoveLogisticsMVC.Models;

namespace MedicMoveLogisticsMVC.Controllers
{
    public class ClientController : Controller
    {

        private readonly ClientService _clientService;

        public ClientController(ClientService clientService)
        {

            _clientService = clientService;
        }

        public async Task<IActionResult> Index()
        {
            var token = HttpContext.Session.GetString("JWTToken");
            var role = HttpContext.Session.GetString("AccountRole");


            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Accounts");
            }

            if (role == "Admin")
            {
                var clients = await _clientService.GetAllClientsAsync(token);
                ViewBag.AccountRole = "Admin"; // Pass the role to the view
                return View(clients);
            }
            else if (role == "Client")
            {
                int? sessionClientId = HttpContext.Session.GetInt32("AccountClientId");

                if (sessionClientId == null)
                {
                    return RedirectToAction("Index", "Accounts");
                            }
                
                var client = await _clientService.GetClientByIdAsync(sessionClientId.Value, token);
                List<Client> clients = new List<Client>();
                if (client != null)
                {
                    clients.Add(client);
                }
                ViewBag.AccountRole = "Client"; // Pass the role to
                                                //Returns the whole list stored in the db, but the client will only see their own information because of the session check in the service method
                return View(clients);
            }
            return RedirectToAction("Login", "Accounts");
        }

        [HttpGet]
        public IActionResult Create()
        {
            var role = HttpContext.Session.GetString("AccountRole");
            if (role != "Admin")
            {
                return RedirectToAction("Index");
            }
            return View(new Client());
        }

        [HttpPost]
        public async Task<IActionResult> Create(Client client)
        {
            var token = HttpContext.Session.GetString("JWTToken");
            var role = HttpContext.Session.GetString("AccountRole");

            if (string.IsNullOrEmpty(token) || role != "Admin")
            {
                return RedirectToAction("Login", "Accounts");
            }
            if (!ModelState.IsValid)
            {
                return View(client);
            }
            await _clientService.CreateClientAsync(client, token);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var token = HttpContext.Session.GetString("JWTToken");
            var role = HttpContext.Session.GetString("AccountRole");
            if (string.IsNullOrEmpty(token) || role != "Admin")
            {
                return RedirectToAction("Login", "Accounts");
            }
            var client = await _clientService.GetClientByIdAsync(id, token);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Client client)
        {
            var token = HttpContext.Session.GetString("JWTToken");
            var role = HttpContext.Session.GetString("AccountRole");

            if (string.IsNullOrEmpty(token) || role != "Admin")
            {
                return RedirectToAction("Login", "Accounts");
            }

            if (!ModelState.IsValid)
            {
                return View(client);
            }
            await _clientService.UpdateClientAsync(id, client, token);
            return RedirectToAction(nameof(Index));
        }
        ////Details for a specific patient
        public async Task<IActionResult> Details(int? id)
        {
            var token = HttpContext.Session.GetString("JWTToken");
            var role = HttpContext.Session.GetString("AccountRole");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Accounts");
            }

            if (id == null)
            {
                return NotFound();
            }

            //Find the client by their id
            var client = await _clientService.GetClientByIdAsync(id.Value, token);
            if (client == null)
            {
                return NotFound();
            }
            ViewBag.AccountRole = role; // Pass the role to the view
            return View(client);

        }

        //Delete Methods
        public async Task<IActionResult> Delete(int id)
        {
            var token = HttpContext.Session.GetString("JWTToken");
            var role = HttpContext.Session.GetString("AccountRole");

            if (string.IsNullOrEmpty(token) || role != "Admin")
            {
                return RedirectToAction("Login", "Accounts");
            }
            var client = await _clientService.GetClientByIdAsync(id, token);
            if (client == null)
            {
                return NotFound();
            }


            return View(client);

        }
        [HttpPost]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var token = HttpContext.Session.GetString("JWTToken");
            var role = HttpContext.Session.GetString("AccountRole");
            var client = await _clientService.GetClientByIdAsync(id, token);
            if (string.IsNullOrEmpty(token) || role != "Admin")
            {
                return RedirectToAction("Login", "Accounts");
            }

            await _clientService.DeleteClientAsync(id, token);

            return RedirectToAction(nameof(Index));
        }
    }
}
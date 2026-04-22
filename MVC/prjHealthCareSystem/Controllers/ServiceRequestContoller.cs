using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using prjHealthCareSystem.Data;
using prjHealthCareSystem.Models;

namespace prjHealthCareSystem.Controllers
{
    public class ServiceRequestController : Controller
    {
        
        //Use HttpClient to call the OpenExchangeRates API.
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;


       private readonly ApplicationDbContext _context;

        public ServiceRequestController(ApplicationDbContext context, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
           
            _context = context;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }
        public async Task<IActionResult> Index()
        {
            var serviceRequests = _context.ServiceRequests.Include(s => s.Contract).ThenInclude(c => c.Client);
                 //IIE Emeris School of Computer Science,2025

 string role = HttpContext.Session.GetString("AccountRole");
 ViewBag.AccountRole = role; // Pass the role to the view
        if (role == "Admin")
            {
                
         var requests = _context.ServiceRequests //DbSet<Contract>
            .Include(c => c.Contract)
            .ThenInclude(c => c.Client)
            .AsQueryable();


             return View(await requests.ToListAsync());

            }

            else if (role == "Client")
            {
                int? accountId = HttpContext.Session.GetInt32("AccountID");

var client =_context.Clients.FirstOrDefault(c => c.AccountId == accountId);

            if (client == null)
            {
            return RedirectToAction("Index", "Accounts");
            }

              var requests = _context.ServiceRequests.Include(s => s.Contract)
              .Where(p => p.Contract.ClientId == client.Id)
                .ToList();
                 return View(requests);
            }
             
            return RedirectToAction("Index", "Accounts");
        

        }
    
        
        //create
        public IActionResult Create()
        {
            ViewData["ContractId"] = new SelectList(_context.Contracts, "Id", "ContractName");
            return View();
        }

       [HttpPost]
        public async Task<IActionResult> Create(ServiceRequest serviceRequest)
        {
           

            var contract = await _context.Contracts.FindAsync((serviceRequest.ContractId));
           
            if(contract == null || contract.Status != "Active")
            {
                var errorMessage = "The selected contract is not active. Please select an active contract.";
                ModelState.AddModelError("ContractId", errorMessage);
            }
           decimal exchangeRate = 0;
            try
            {
                var strategy = CurrencyFactory.Create(serviceRequest.CurrencyFrom, _httpClientFactory, _configuration);
                exchangeRate = await strategy.GetRateAsync();
                
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError("CurrencyFrom", ex.Message);
            }
            if(ModelState.IsValid)
            {
                
                serviceRequest.Cost= serviceRequest.Amount * exchangeRate;
                _context.Add(serviceRequest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ContractId"] = new SelectList(_context.Contracts, "Id", "ContractName", serviceRequest.ContractId);
           
     return View(serviceRequest);
        }
[HttpPost]
        public async Task<IActionResult> Edit(int id, ServiceRequest serviceRequest)
        {
    
            if(ModelState.IsValid)
            {
                _context.Update(serviceRequest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ContractId"] = new SelectList(_context.Contracts, "Id", "ContractName", serviceRequest.ContractId);
            return View(serviceRequest);
        }

//Delete
        public async Task<IActionResult> Delete(int? id)
        {
            var serviceRequest = _context.ServiceRequests
            .Include(s => s.Contract)
            .FirstOrDefaultAsync(s => s.Id == id);
            return View(await serviceRequest);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var serviceRequest = await _context.ServiceRequests.FindAsync(id);
            if(serviceRequest != null)
            {
                _context.ServiceRequests.Remove(serviceRequest);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
    }


    //Lastt Onneee
    public async Task<IActionResult> Details(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var serviceRequest = _context.ServiceRequests
            .Include(s => s.Contract)
            .ThenInclude(c => c.Client)
            .FirstOrDefaultAsync(m => m.Id == id); 

            
            if(serviceRequest == null)
            {
                return NotFound();
            }
            return View(await serviceRequest);
}
    }

}
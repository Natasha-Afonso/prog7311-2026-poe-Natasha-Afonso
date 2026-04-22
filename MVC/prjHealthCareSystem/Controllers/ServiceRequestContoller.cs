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

private readonly Dictionary<string, ICurrencyStrategy> _currencyStrategies;
       private readonly ApplicationDbContext _context;

        public ServiceRequestController(ApplicationDbContext context, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _currencyStrategies = new Dictionary<string, ICurrencyStrategy>
            {
                { "USD", new USDStrategy(httpClientFactory, configuration) },
                { "CNY", new CNYStrategy(httpClientFactory, configuration) },
                { "JPY", new JPYStrategy(httpClientFactory, configuration) }
            };
            _context = context;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }
        public async Task<IActionResult> Index()
        {
            var serviceRequests = _context.ServiceRequests.Include(s => s.Contract).ThenInclude(c => c.Client);
                 //IIE Emeris School of Computer Science,2025

            return View(await serviceRequests.ToListAsync());

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
           

            var policy = await _context.Contracts.FindAsync((serviceRequest.ContractId));
           
            if(policy == null || policy.Status != "Active")
            {
                var errorMessage = "The selected contract is not active. Please select an active contract.";
                ModelState.AddModelError("ContractId", errorMessage);
            }

            if(!_currencyStrategies.ContainsKey(serviceRequest.CurrencyFrom))
            {
                var errorMessage = "The selected currency is not supported. Please select a supported currency.";
                ModelState.AddModelError("Currency", errorMessage);
            }
            if(ModelState.IsValid)
            {
                var strategy = _currencyStrategies[serviceRequest.CurrencyFrom];

                decimal exchangeRate = await strategy.GetRateAsync();
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
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
       private readonly ApplicationDbContext _context;

        public ServiceRequestController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var serviceRequests = _context.ServiceRequests.Include(s => s.Contract).ThenInclude(c => c.Client);

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
            if(ModelState.IsValid)
            {
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
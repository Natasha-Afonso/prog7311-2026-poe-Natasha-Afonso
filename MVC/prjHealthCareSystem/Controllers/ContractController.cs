using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Experimental.BuildCheck;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using prjHealthCareSystem.Data;
using prjHealthCareSystem.Models;

namespace prjHealthCareSystem.Controllers
{
    public class ContractController : Controller
    {
        //Adding the data class
        private readonly ApplicationDbContext _context;

        public ContractController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(DateTime? startTime, DateTime? endDateTime, string status, string signedConsentPath)
        {
        string role = HttpContext.Session.GetString("AccountRole");
        if (role == "Admin")
            {
                
         var search = _context.Contracts //DbSet<Contract>
            .Include(c => c.Client).AsQueryable();

         
          //Search date
          if (startTime.HasValue && endDateTime.HasValue)
            {
                search = search.Where(p => 
                p.StartDate >= startTime && p.EndDate <= endDateTime); //IQueryable<Contract>
            }

            //Search Status
            if(!string.IsNullOrEmpty(status))
            {
                search= search.Where(p => p.Status == status);

            }
ViewBag.StartTime = startTime?.ToString("yyyy-MM-dd");
ViewBag.EndDateTime = endDateTime?.ToString("yyyy-MM-dd");
             return View(await search.ToListAsync());

            }

            else if (role == "Client")
            {
                int? accountId = HttpContext.Session.GetInt32("AccountID");

var client =_context.Clients.FirstOrDefault(c => c.AccountId == accountId);

            if (client == null)
            {
            return RedirectToAction("Index", "Accounts");
            }

              var contracts = _context.Contracts.Where(c => c.ClientId == accountId)
                .ToList();
                 return View(contracts);
            }
             
            return RedirectToAction("Index", "Accounts");
        
    }



        public IActionResult Create()
        {
                 ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Name");
            return View();
        }


    [HttpPost]
            public async Task<IActionResult> Create(Contract contract, IFormFile? upload)
            {
                if(ModelState.IsValid)
                {
                    if(upload != null && upload.Length > 0)
                    {
                       var uploadsfolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                        if(!Directory.Exists(uploadsfolder))
                        {
                            Directory.CreateDirectory(uploadsfolder);
                        }

                        //Create a file name so that there is no overwrite
                        var fileName = Guid.NewGuid().ToString() + "_"+(upload.FileName);
                        var filePath = Path.Combine(uploadsfolder, fileName);

                       
                        //Open a pipeline to read the file to the hard drive
                       var stream = new FileStream(filePath, FileMode.Create);
                        using(stream)
                    {
                        await upload.CopyToAsync(stream);
                    }
                    //Save the text path to the database so we can find the file later alligator
                    contract.SignedConsentPath = "/uploads/" + fileName;
            }
            //Add the new policy to the database
            _context.Add(contract);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
                
            
            }
 //Repopulate the patient dropdown so t doesn't break when we reload the page
    ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Name", contract.ClientId);
             
        //Send back to the form
            return View(contract);


            }
            
        //Edit
        public async Task<IActionResult> Edit(int id, Contract contract)
        {
            if (ModelState.IsValid)
            {
                _context.Update(contract);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }


            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Name",
             contract.ClientId);
            return View(contract);
        }

        //Delete
        public async Task<IActionResult> Delete(int? id)
        {
            var deleteContract = await _context.Contracts.//DbSet<InsurancePolicy>
            Include(p => p.Client)//Task<InsurancePolicy>
            .FirstOrDefaultAsync(m => m.Id == id);
            return View(deleteContract);
        }

        [HttpPost, ActionName("Delete")]
     public async Task<IActionResult> DeleteConfirm(int id)
        {
            var contract =await _context.Contracts.FindAsync(id);
            if(contract != null)
            {
                _context.Contracts.Remove(contract);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
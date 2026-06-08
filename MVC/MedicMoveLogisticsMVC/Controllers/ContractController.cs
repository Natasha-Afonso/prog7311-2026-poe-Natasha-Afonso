using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MedicMoveLogisticsMVC.Data;
using MedicMoveLogisticsMVC.Models;
using MedicMoveLogisticsMVC.Services;

namespace MedicMoveLogisticsMVC.Controllers
{
    public class ContractController : Controller
    {
       
    
        private readonly ContractService _contractService;
        private readonly ClientService _clientService;
        private readonly IWebHostEnvironment _webEnvironment;
        public ContractController(ContractService contractService, ClientService clientService, IWebHostEnvironment webEnvironment)
        {
           
            _contractService = contractService;
            _clientService = clientService;
            _webEnvironment = webEnvironment;
        }

// GET: Contract
        public async Task<IActionResult> Index(DateTime? startTime, DateTime? endDateTime, string status, string signedConsentPath)
        {
            string role = HttpContext.Session.GetString("AccountRole");
            ViewBag.AccountRole = role; // Pass the role to the view
             //extract token from session
            var token = HttpContext.Session.GetString("JWTToken") ?? "";
            //pull the contracts from the API
            var allcontracts = await _contractService.GetAllContractsAsync(token);
            var search = allcontracts.AsQueryable();
            
           
            if (role == "Admin")
            {
               
                //Search date
                if (startTime.HasValue && endDateTime.HasValue)
                {
                    search = search.Where(p =>
                    p.StartDate >= startTime && p.EndDate <= endDateTime); //IQueryable<Contract>
                }


                //Search Status
                if (!string.IsNullOrEmpty(status))
                {
                    search = search.Where(p => p.Status == status);

                }

                ViewBag.StatusFilter = new SelectList(new[]
      { "Draft", "Active", "Expired" }, status); // Pass the selected status to the view
                ViewBag.AvailableOptions = status; //Ensures the dropdown retains the selected value after form submission
                ViewBag.StartTime = startTime?.ToString("yyyy-MM-dd");
                ViewBag.EndDateTime = endDateTime?.ToString("yyyy-MM-dd");
                
                return View(search.ToList());
                 
            }

            else if (role == "Client")
            {
                int? sessionClientId = HttpContext.Session.GetInt32("AccountClientId");
                var allClients = await _clientService.GetAllClientsAsync(token);
                var client = allClients.FirstOrDefault(c => c.ContactDetails == HttpContext.Session.GetString("UserEmail"))?.Id;

                if (client == null)
                {
                    return RedirectToAction("Index", "Accounts");
                }

                var contracts = search.Where(c => c.ClientId == sessionClientId.Value).ToList();
                
                return View(contracts);
            }

            return RedirectToAction("Index", "Accounts");

        }


// GET: Create
        public async Task<IActionResult> Create()
        {
            var token = HttpContext.Session.GetString("JWTToken") ?? "";
            var clients= await _clientService.GetAllClientsAsync(token);
            ViewData["ClientId"] = new SelectList(clients, "Id", "Name");
            return View();
        }

//POST: Create
        [HttpPost]
        public async Task<IActionResult> Create(Contract contract, IFormFile? upload)
        {
              var token = HttpContext.Session.GetString("JWTToken") ?? "";
            
            if (ModelState.IsValid)
            {
                if (upload != null && upload.Length > 0)
                {
                    var uploadsfolder = Path.Combine(_webEnvironment.WebRootPath, "uploads");
                   if (!Directory.Exists(uploadsfolder))
                    {
                        Directory.CreateDirectory(uploadsfolder);
                    }

                    //Create a file name so that there is no overwrite
                    var fileName = Guid.NewGuid().ToString() + "_" + (upload.FileName);
                    var filePath = Path.Combine(uploadsfolder, fileName);


                    //Open a pipeline to read the file to the hard drive
                    
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await upload.CopyToAsync(stream);
                    }
                    //Save the text path to the database so we can find the file later alligator
                    contract.SignedConsentPath = "/uploads/" + fileName;
                }
                try
                {
                    await _contractService.CreateContractAsync(contract,token);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("",$"API Error: {ex.Message}");
                }
            }
            var clients = await _clientService.GetAllClientsAsync(token);
            ViewData["ClientId"] = new SelectList(clients, "Id", "Name", contract.ClientId);
            return View(contract);
        }

        //Get: Edit
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var token = HttpContext.Session.GetString("JWTToken") ?? "";
                var contract = await _contractService.GetContractByIdAsync(id,token);
                if (contract == null)
                return NotFound();
                
                 
            var clients= await _clientService.GetAllClientsAsync(token);
                ViewData["ClientId"] = new SelectList(clients, "Id", "Name", contract.ClientId);
                return View(contract);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(500, $"API Error: {ex.Message}");
            }
        }
        //Edit
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Contract contract)
        {
            if (id != contract.Id)
            return NotFound();
  var token = HttpContext.Session.GetString("JWTToken") ?? "";
          
            if (ModelState.IsValid)
            {
                try
                {
                    await _contractService.UpdateContractAsync(id, contract,token);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Failed to update contract: {ex.Message}");
                    
                }
            }
                var clients = await _clientService.GetAllClientsAsync(token);
                ViewData["ClientId"] = new SelectList(clients, "Id", "Name", contract.ClientId);
                return View(contract);
            }



        //GET Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            try
            {
                var token = HttpContext.Session.GetString("JWTToken") ?? "";
                var contract = await _contractService.GetContractByIdAsync(id.Value,token);
                if (contract == null) return NotFound();
                return View(contract);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return NotFound($"API Error: {ex.Message}");
            }
        }
         

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            try
            {
                var token = HttpContext.Session.GetString("JWTToken") ?? "";
                await _contractService.DeleteContractAsync(id,token);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Failed to delete contract: {ex.Message}";
            return RedirectToAction(nameof(Index));
        }
    }
}
}
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MedicMoveLogisticsMVC.Data;
using MedicMoveLogisticsMVC.Models;
using MedicMoveLogisticsMVC.Services;



namespace MedicMoveLogisticsMVC.Controllers
{
    public class ServiceRequestsController : Controller
    {

        private readonly ServiceRequestsService _serviceRequestService;
        private readonly ClientService _clientService;
        private readonly ContractService _contractService;
        public ServiceRequestsController(ServiceRequestsService serviceRequestsService, ClientService clientService, ContractService contractService)
        {
            _contractService = contractService;
            _clientService = clientService;
            _serviceRequestService = serviceRequestsService;
        }
        public async Task<IActionResult> Index()
        {
            //IIE Emeris School of Computer Science,2025
            string role = HttpContext.Session.GetString("AccountRole");
            ViewBag.AccountRole = role; // Pass the role to the view

            var token = HttpContext.Session.GetString("JWTToken") ?? "";
            var allRequests = await _serviceRequestService.GetAllServiceRequestsAsync(token);
            var search = allRequests.AsQueryable();

            if (role == "Admin")
            {
                return View(search.ToList());
            }
            else if (role == "Client")
            {
                //Pull the active client id saved in session
                int? sessionClientId = HttpContext.Session.GetInt32("AccountClientId");


                if (sessionClientId == null)
                {
                    var allClients = await _clientService.GetAllClientsAsync(token);
                    sessionClientId = allClients.FirstOrDefault(c => c.ContactDetails == HttpContext.Session.GetString("UserEmail"))?.Id;
                }

                if (sessionClientId == null)
                {
                    return RedirectToAction("Index", "Accounts");
                }

                var clientRequests = search.Where(r => r.Contract != null && r.Contract.ClientId == sessionClientId.Value).ToList();
                return View(clientRequests);
            }
            return RedirectToAction("Index", "Accounts");
        }

        //create
        public async Task<IActionResult> Create()
        {
            var token = HttpContext.Session.GetString("JWTToken") ?? "";

            var contracts = await _contractService.GetAllContractsAsync(token);

           ViewBag.ContractId = new SelectList(contracts, "Id", "ContractName");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ServiceRequests serviceRequest)
        {
            var token = HttpContext.Session.GetString("JWTToken") ?? "";
            if (ModelState.IsValid)
            {
                try
                {
                 if (serviceRequest.Condition == "Cold-Chain (<9 degrees Celcius)")
                    {
                    
                        serviceRequest.AssignedVehicleType = "Refrigerated Truck";
                    }
                    else if (serviceRequest.PackageSize == "Large")
                    {
                        serviceRequest.AssignedVehicleType = "Heavy Cargo Van";
                    }
                    else if (serviceRequest.Speed == "Rush" || serviceRequest.Speed == "Express")
                    {
                        serviceRequest.AssignedVehicleType = "Express Vehicle";
                    }
                    else
                    {
                        serviceRequest.AssignedVehicleType = "Standard Vehicle";
                    }
                    serviceRequest.Status = "Pending Approval";
                    await _serviceRequestService.CreateServiceRequestsAsync(serviceRequest, token);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"API ERROR:{ex.Message}");
                }
            }


            var contracts = await _contractService.GetAllContractsAsync(token);
            ViewBag.ContractId = new SelectList(contracts, "Id", "ContractName", serviceRequest.ContractId);

            return View(serviceRequest);
        }
        //Saha, D.2024
        //Speed dropdown if contract is premium
        [HttpPost]
        public async Task<JsonResult> GetSpeedOptions(string contractId)
        {
            var token = HttpContext.Session.GetString("JWTToken") ?? "";
               var optionsList =new List<object>();
               if (!string.IsNullOrEmpty(contractId))
            {
                int id = Convert.ToInt32(contractId);
                var chosenContract = await _contractService.GetContractByIdAsync(id,token);
            
            //if contract is basinc restrict options
            if (chosenContract != null && chosenContract.ServiceLevel == "Basic")
                {
                    optionsList.Add(new{ value = "Standard", text = "Standard Delivery (Contract Limit)"});
                }
                else
                {
                    optionsList.Add(new {value ="Standard", text = "Standard Delivery"});
                     optionsList.Add(new {value ="Express", text = "Express Delivery"});
                     optionsList.Add(new {value ="Rush", text = "Rush High priority"});
                }
            }
            return Json(optionsList);
        }

        //GET EDIT
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var token = HttpContext.Session.GetString("JWTToken") ?? "";
                var serviceRequest = await _serviceRequestService.GetServiceRequestsByIdAsync(id, token);
                if (serviceRequest == null) return NotFound();
                return View(serviceRequest);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"API ERROR: {ex.Message}");
            }
        }

        //POST 
        [HttpPost]
        public async Task<IActionResult> Edit(int id, ServiceRequests serviceRequest)
        {
            if (id != serviceRequest.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var token = HttpContext.Session.GetString("JWTToken") ?? "";
                    await _serviceRequestService.UpdateServiceRequestsAsync(id, serviceRequest, token);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Failed to update via API: {ex.Message}");
                }
            }
            return View(serviceRequest);
        }

        //GET DELETE
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();
            try
            {
                var token = HttpContext.Session.GetString("JWTToken") ?? "";
                var serviceRequest = await _serviceRequestService.GetServiceRequestsByIdAsync(id.Value, token);
                if (serviceRequest == null) return NotFound();
                return View(serviceRequest);
            }
            catch (Exception ex)
            {
                return NotFound($"API ERROR: {ex.Message}");
            }
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            try
            {
                var token = HttpContext.Session.GetString("JWTToken") ?? "";
                await _serviceRequestService.DeleteServiceRequestsAsync(id, token);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Failed to delete the request through the AP: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        //GET Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            try
            {
                var token = HttpContext.Session.GetString("JWTToken") ?? "";
                var serviceRequest = await _serviceRequestService.GetServiceRequestsByIdAsync(id.Value, token);
                if (serviceRequest == null) return NotFound();

                return View(serviceRequest);

            }
            catch (Exception ex)
            {

                return NotFound($"API ERROR: {ex.Message}");
            }
        }
    }

}

/*References
Saha, D.2024. Implement Cascading Dropdown In ASP.NET Core MVC Application Using InMemory DB.(Version 10)[source code]. Available at: < https://www.c-sharpcorner.com/article/implement-cascading-dropdown-in-asp-net-core-mvc-application-using-inmemory-db/ > [Accessed 7 June 2026]
 */


